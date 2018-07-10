namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

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
        private List<DataSet> openDataSets;

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

        public DataListTreeView(TreeViewState treeViewState, List<DataSet> openDataSets, DataSet activeDataSet)
            : base(treeViewState)
        {
            this.showAlternatingRowBackgrounds = true;
            this.openDataSets = openDataSets;
            this.activeDataSet = activeDataSet;
        }

        public void SetActiveDataSet(DataSet dataSet)
        {
            this.activeDataSet = dataSet;
        }

        public void SelectLastItem(Data item)
        {
            var id = this.idToDataMap.IndexOf(item);
            //this.FrameItem(id);
            this.SetSelection(new[]{id}, TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);
            //this.SelectionClick(last, false);
            //Selection.objects = new[] { this.idToDataMap[this.idToDataMap.Count - 1] };
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

            if (this.openDataSets.Count == 0)
            {
                root.children = new List<TreeViewItem>();
                return root;
            }
            
            foreach (var dataSet in this.openDataSets)
            {
                var dataSetNode = new TreeViewItem { id = index, displayName = dataSet.name, icon = dataSet.Icon };

                this.idToDataMap.Add(dataSet);
                this.dataSetTreeIds.Add(index);
                root.AddChild(dataSetNode);
                index++;

                index = dataSet.Children
                    .Aggregate(index, (current, data) => this.AddEntity(data as Data, dataSetNode, current));
            }

            TreeView.SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            // Lock the inspector to the selected entities so that we can edit the scene proxies without changing the Inspector.
            ActiveEditorTracker.sharedTracker.isLocked = false;
            Selection.objects = (from id in selectedIds select this.idToDataMap[id]).ToArray();
            ActiveEditorTracker.sharedTracker.isLocked = true;
            
            // Replace any selection of TransformDatas with their scene proxies.
            // TODO: Handle null transforms
            Selection.objects =
                (from obj in Selection.objects
                 select (obj as TransformData)?.SceneProxyTransform.gameObject ?? obj)
                 .ToArray();
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

        public void RemoveDataSet(DataSet dataSet)
        {
            var id = this.idToDataMap.IndexOf(dataSet);
            this.RemoveDataSet(id);
        }

        public void SelectDataSet(DataSet dataSet)
        {
            var id = this.idToDataMap.IndexOf(dataSet);
            this.SetSelection(new List<int> { id });
        }

        private int AddEntity(Data entity, TreeViewItem parent, int id)
        {
            if (entity == null || this.idToDataMap.Contains(entity))
            {
                return id;
            }
            
            if (entity.Parent != this.idToDataMap[parent.id] && entity.Parent != null)
            {
                return id;
            }
            
            var node = new TreeViewItem { id = id, displayName = entity.Name, icon = entity.Icon };

            this.idToDataMap.Add(entity);
            parent.AddChild(node);
            id++;

            return entity.Children
                .Aggregate(id, (current, child) => this.AddEntity(child as Data, node, current));
        }

        private void RemoveDataSet(object id)
        {
            var dataSetId = (int)id;
            var dataSet = this.idToDataMap[dataSetId] as DataSet;
            Assert.IsNotNull(dataSet);
            
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