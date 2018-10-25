namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

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
        private List<string> openDataSetGuids;

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

        /// <summary>
        /// For a given tree item ID, what DataSetAsset does it belong to?
        /// </summary>
        [SerializeField]
        private List<DataSetAsset> idToDataSetMap = new List<DataSetAsset>();
        
        [SerializeField]
        private DataSet activeDataSet;

        public delegate SceneProxy GetSceneProxyDelegate(string dataSetGuid, string entityName);

        private readonly GetSceneProxyDelegate getSceneProxy;

        public DataListTreeView(TreeViewState treeViewState, List<string> openDataSetGuids, DataSet activeDataSet, GetSceneProxyDelegate getSceneProxy)
            : base(treeViewState)
        {
            this.showAlternatingRowBackgrounds = true;
            this.openDataSetGuids = openDataSetGuids;
            this.activeDataSet = activeDataSet;
            this.getSceneProxy = getSceneProxy;
        }

        public void SetActiveDataSet(DataSet dataSet)
        {
            this.activeDataSet = dataSet;
        }

        public void SelectItem(Data item)
        {
            int id;
            var foundItem = this.TryGetSelectedIdFromData(item, out id);

            if (!foundItem)
            {
                return;
            }

            this.SetSelection(new[]{id}, TreeViewSelectionOptions.FireSelectionChanged | TreeViewSelectionOptions.RevealAndFrame);

            this.Repaint();
        }

        private bool TryGetSelectedIdFromData(Data item, out int id)
        {
            id = 0;
            foreach (var entry in this.idToDataMap)
            {
                if (entry != null && entry.DataSetGuid == item.DataSetGuid && entry.Name == item.Name)
                {
                    return true;
                }

                id++;
            }

            return false;
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
                    SingletonScriptableObject<DataListWindowState>.Instance.DeleteSceneProxy(this.activeDataSet.DataSetGuid, data.Name, DataListWindowState.DestroyGameObject.Destroy);
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
            
            if (entity is DataSet)
            {
                var path = AssetDatabase.GUIDToAssetPath(entity.DataSetGuid);
                AssetDatabase.RenameAsset(path, args.newName + ".asset");
                return;
            }

            entity.Name = args.newName;

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
                var transformData = this.idToDataMap[id] as TransformData;
                var sceneProxy = this.getSceneProxy(transformData.DataSetGuid, transformData.Name);

                var sceneProxyPosition = sceneProxy.transform.position;
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
            this.idToDataSetMap.Clear();

            var index = 1;
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "root" };

            // ID 0 is the root, not an actual Data reference.
            this.idToDataMap.Add(null);
            this.idToDataSetMap.Add(null);

            if (this.openDataSetGuids.Count == 0)
            {
                root.children = new List<TreeViewItem>();
                return root;
            }
            
            foreach (var dataSetGuid in this.openDataSetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(dataSetGuid);
                var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(path);
                if (dataSet == null)
                {
                    Debug.LogWarning($"DataSet {path} could not be loaded or does not exist.");
                    continue;
                }

                var dataSetNode = new TreeViewItem { id = index, displayName = dataSet.name, icon = dataSet.GetDataSet().Icon };

                this.idToDataMap.Add(dataSet.GetDataSet());
                this.idToDataSetMap.Add(dataSet);
                this.dataSetTreeIds.Add(index);
                root.AddChild(dataSetNode);
                index++;
                
                index = dataSet.GetDataSet().GetDataList().Values
                    .Aggregate(index, (current, data) => this.AddData(dataSet, data, dataSetNode, current));
            }

            TreeView.SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        private int AddData(DataSetAsset asset, Data data, TreeViewItem parent, int id)
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
                Debug.LogError($"Attempted to add duplicate Entity '{data.Name}' to DataListTreeView.");
            }

            var node = new TreeViewItem { id = id, displayName = data.Name, icon = data.Icon };

            this.idToDataMap.Add(data);
            this.idToDataSetMap.Add(asset);
            parent.AddChild(node);
            id++;
            
            if (transformData == null)
            {
                return id;
            }

            return transformData.GetChildren()
                .Where(child => child != null)  // Stupidly, a child can be null, thanks to our friend ombs
                .Aggregate(id, (current, child) => this.AddData(asset, child, node, current));
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            // TODO handle multiple selections
            var selected = (from id in selectedIds select this.idToDataMap[id]).ToArray();
            
            if (selected.Length == 0)
            {
                FoxKitEditor.InspectedEntity = null;
                return;
            }

            FoxKitEditor.InspectedEntity = selected[0];

            var dataSets = (from id in selectedIds select this.idToDataSetMap[id]).ToArray();
            Selection.objects = dataSets;
            
            // Lock the inspector to the selected entities so that we can edit the scene proxies without changing the Inspector.
            ActiveEditorTracker.sharedTracker.isLocked = false;
            Selection.objects = (from id in selectedIds
                                 select AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(this.idToDataMap[id].DataSetGuid)))
                                 .ToArray();
            ActiveEditorTracker.sharedTracker.isLocked = true;
            
            // For each TransformData selected, select its scene proxy.
            var newSelection = new List<UnityEngine.Object>();
            foreach (var id in selectedIds)
            {
                var data = this.idToDataMap[id];
                var transformData = data as TransformData;

                if (transformData == null)
                {
                    var dataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(data.DataSetGuid));
                    newSelection.Add(dataSet);
                    continue;
                }

                var sceneProxy = this.getSceneProxy(data.DataSetGuid, data.Name);
                newSelection.Add(sceneProxy.gameObject);
            }

            Selection.objects = newSelection.ToArray();
        }

        protected override void ContextClickedItem(int id)
        {
            // For the time being, we only care about right clicking on the DataSet, not its children.
            // So, don't open the menu if the user didn't right click on a DataSet.
            var selectedDataSets = from treeId in this.GetSelection()
                                   where this.dataSetTreeIds.Contains(treeId)
                                   select this.idToDataMap[treeId] as DataSet;


            var clickedDataSet = this.idToDataMap[id] as DataSet;

            DataListWindow.GetInstance()
                .MakeShowItemContextMenuDelegate()(clickedDataSet, selectedDataSets.ToList());
        }
        
        public void SelectDataSet(DataSet dataSet)
        {
            var id = this.idToDataMap.IndexOf(dataSet);
            this.SetSelection(new List<int> { id });
        }
        
        public void RemoveDataSet(object id)
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