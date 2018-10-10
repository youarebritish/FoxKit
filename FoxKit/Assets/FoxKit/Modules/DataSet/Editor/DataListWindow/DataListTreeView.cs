namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class DataListTreeView : TreeView
    {
        /// <summary>
        /// Width of icons.
        /// </summary>
        private const float IconWidth = 16f;

        /// <summary>
        /// Amount of horizontal space between icons and labels.
        /// </summary>
        private const float SpaceBetweenIconAndText = 2f;

        /// <summary>
        /// The currently open DataSets.
        /// </summary>
        [SerializeField]
        private List<string> openDataSetPaths;

        /// <summary>
        /// The index of this list is the tree item ID of a given Data.
        /// </summary>
        [SerializeField]
        private List<Data> idToDataMap = new List<Data>();

        /// <summary>
        /// Tree view IDs of DataSet entries.
        /// </summary>
        [SerializeField]
        private List<int> dataSetTreeIds = new List<int>();
        
        private DataSet activeDataSet;

        public DataListTreeView(TreeViewState treeViewState, List<string> openDataSetPaths, DataSet activeDataSet)
            : base(treeViewState)
        {
            this.showAlternatingRowBackgrounds = true;
            this.openDataSetPaths = openDataSetPaths;
            this.activeDataSet = activeDataSet;
        }

        public void SetActiveDataSet(DataSet dataSet)
        {
            this.activeDataSet = dataSet;
        }

        public void SelectItem(Data item)
        {
            // TODO Scroll to this item
            var id = this.idToDataMap.IndexOf(item);
            this.SetSelection(new[]{id}, TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
        }

        public void HandleDelete()
        {
            var selected = this.GetSelection();

            // Only remove a DataSet if all selected are DataSets. Otherwise, delete the selected Entities.
            if (selected.Any(item => !(this.idToDataMap[item] is DataSet)))
            {
                foreach (var item in selected)
                {
                    var data = this.idToDataMap[item];
                    this.activeDataSet.RemoveData(data.Name);
                }

                AssetDatabase.Refresh();
                this.Reload();
            }
            else
            {
                foreach (var item in selected)
                {
                    var dataSet = this.idToDataMap[item] as DataSet;
                    this.RemoveDataSet(dataSet);
                }

                this.Reload();
            }
        }

        protected override bool CanRename(TreeViewItem item)
        {
            return true;
        }

        protected override void RenameEnded(RenameEndedArgs args)
        {
            base.RenameEnded(args);

            if (!args.acceptedRename)
            {
                return;
            }

            var entity = this.idToDataMap[args.itemID];
            
            entity.Name = args.newName;
            
            if (entity is DataSet)
            {
                return;
            }
            
            // If we've renamed a Data, change its key.
            entity.GetDataSet().RemoveData(args.originalName);
            entity.GetDataSet().AddData(args.newName, entity);

            this.Reload();
        }

        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);

            // If the user double clicked a DataSet, then make it active.
            if (this.dataSetTreeIds.Contains(id))
            {
                DataListWindow.GetInstance().SetActiveDataSet(this.idToDataMap[id]);
            }
            else if (this.idToDataMap[id] is TransformData)
            {
                if (SceneView.lastActiveSceneView == null)
                {
                    Debug.LogWarning("SceneView.lastActiveSceneView is null. Click in the Scene view to fix this.");
                    return;
                }

                // If the user double clicked a TransformData, navigate to its scene proxy.
                var sceneProxyPosition = ((TransformData)this.idToDataMap[id]).SceneProxyTransform.position;
                SceneView.lastActiveSceneView.LookAt(sceneProxyPosition);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var useBoldFont = this.activeDataSet == this.idToDataMap[args.item.id];
            this.OnContentGUI(args.rowRect, args.item, args.label, args.selected, args.focused, useBoldFont);
        }

        protected override TreeViewItem BuildRoot()
        {
            this.idToDataMap.Clear();

            var index = 1;
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "root" };

            // ID 0 is the root, not an actual Data reference.
            this.idToDataMap.Add(null);

            if (this.openDataSetPaths.Count == 0)
            {
                root.children = new List<TreeViewItem>();
                return root;
            }
            
            foreach (var dataSetPath in this.openDataSetPaths)
            {
                var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(dataSetPath);
                if (dataSet == null)
                {
                    Debug.LogWarning($"DataSet {dataSetPath} could not be loaded or does not exist.");
                    continue;
                }

                var dataSetNode = new TreeViewItem { id = index, displayName = dataSet.name, icon = dataSet.GetDataSet().Icon };

                this.idToDataMap.Add(dataSet.GetDataSet());
                this.dataSetTreeIds.Add(index);
                root.AddChild(dataSetNode);
                index++;
                
                index = dataSet.GetDataSet().GetDataList().Values
                    .Aggregate(index, (current, data) => this.AddData(data, dataSetNode, current));
            }

            TreeView.SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        private int AddData(Data data, TreeViewItem parent, int id)
        {
            // If we're adding a TransformData entity that has a valid parent, only add it to the tree under its parent.
            // TODO: Consider moving this out and checking for this case before calling AddData().
            var transformData = data as TransformData;
            if (transformData?.Parent != null && transformData.Parent != this.idToDataMap[parent.id])
            {
                return id;
            }

            if (this.idToDataMap.Contains(data))
            {
                Assert.IsTrue(false, $"Attempted to add duplicate {data.Name} to DataListTreeView.");
            }

            var node = new TreeViewItem { id = id, displayName = data.Name, icon = data.Icon };

            this.idToDataMap.Add(data);
            parent.AddChild(node);
            id++;
            
            if (transformData == null)
            {
                return id;
            }

            return transformData.GetChildren()
                .Aggregate(id, (current, child) => this.AddData(child, node, current));
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            // TODO handle multiple selections
            var selected = (from id in selectedIds select this.idToDataMap[id]).ToArray();
            if (selected.Length == 0)
            {
                FoxKitEditor.InspectedEntity = null;
            }

            FoxKitEditor.InspectedEntity = selected[0];

            // TODO refresh inspector somehow

            // TODO 
            // Lock the inspector to the selected entities so that we can edit the scene proxies without changing the Inspector.
            /*ActiveEditorTracker.sharedTracker.isLocked = false;
            Selection.objects = (from id in selectedIds select this.idToDataMap[id]).ToArray();
            ActiveEditorTracker.sharedTracker.isLocked = true;*/

            // Replace any selection of TransformDatas with their scene proxies.
            // TODO: Handle null transforms
            /*Selection.objects =
                (from obj in Selection.objects
                 select (obj as TransformData)?.SceneProxyTransform.gameObject ?? obj)
                 .ToArray();*/
        }

        protected override void ContextClickedItem(int id)
        {
            // For the time being, we only care about right clicking on the DataSet, not its children.
            // So, don't open the menu if the user didn't right click on a DataSet.
            if (!this.dataSetTreeIds.Contains(id))
            {
                return;
            }

            var dataSet = this.idToDataMap[id] as DataSet;
            DataListWindow.GetInstance().MakeShowItemContextMenuDelegate()(dataSet);
        }

        public void RemoveDataSet()
        {
            this.Reload();
            // TODO: Does any of this actually do anything?
            /*
            var id = this.idToDataMap.IndexOf(dataSet);
            this.RemoveDataSet(id);*/
        }

        public void SelectDataSet(DataSet dataSet)
        {
            var id = this.idToDataMap.IndexOf(dataSet);
            this.SetSelection(new List<int> { id });
        }
        
        private void RemoveDataSet(object id)
        {
            // TODO: Does this actually do anything?
            /*var dataSetId = (int)id;
            var dataSet = this.idToDataMap[dataSetId] as DataSet;
            Assert.IsNotNull(dataSet);*/
            
            this.Reload();
        }

        private void OnContentGUI(Rect rect, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont)
        {
            if (Event.current.rawType != EventType.Repaint)
            {
                return;
            }
            
            var indent = this.GetContentIndent(item) + this.extraSpaceBeforeIconAndLabel;
            rect.xMin += indent;

            var lineStyle = useBoldFont ? DefaultStyles.boldLabel : DefaultStyles.label;

            // Draw icon.
            var iconRect = rect;
            iconRect.width = IconWidth;

            var icon = item.icon;
            if (icon != null)
            {
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }

            // Draw text.
            lineStyle.padding.left = 0;

            if (icon != null)
            {
                rect.xMin += IconWidth + SpaceBetweenIconAndText;
            }

            lineStyle.Draw(rect, label, false, false, selected, focused);
        }
    }
}