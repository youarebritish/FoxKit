namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Utils;

    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

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
        private List<EntityFileAsset> idToDataSetMap = new List<EntityFileAsset>();
        
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
                    var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(data.DataSetGuid));
                    dataSet.GetDataSet().RemoveData(data.Name);

                    var dataListWindowState = SingletonScriptableObject<DataListWindowState>.Instance;
                    dataListWindowState.DeleteSceneProxy(data.DataSetGuid, data.Name, DataListWindowState.DestroyGameObject.Destroy);

                    if (dataListWindowState.InspectedEntity == data)
                    {
                        dataListWindowState.InspectedEntity = null;
                    }
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

                if (sceneProxy == null)
                {
                    return;
                }

                SceneView.lastActiveSceneView.FrameSelected();
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
                var dataSet = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(path);
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

        private int AddData(EntityFileAsset asset, Data data, TreeViewItem parent, int id)
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

        public void UpdateSelection()
        {
            this.SelectionChanged(this.GetSelection());
        }
        
        protected override bool CanStartDrag(TreeView.CanStartDragArgs args)
        {
            return !args.draggedItemIDs.Select(draggedID => this.idToDataMap[draggedID]).Any(data => data is DataSet);
        }

        protected override void SetupDragAndDrop(TreeView.SetupDragAndDropArgs args)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.paths = null;
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.SetGenericData("Items", new List<int>(args.draggedItemIDs));
            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            DragAndDrop.StartDrag("DataListTreeView");
        }

        protected override DragAndDropVisualMode HandleDragAndDrop(TreeView.DragAndDropArgs args)
        {
            var target = args.parentItem;
            if (target == null)
            {
                return DragAndDropVisualMode.Rejected;
            }

            var targetData = this.idToDataMap[target.id];

            // Eventually handle moving to other DataSets, but not yet
            if (!(targetData is TransformData))
            {
                return DragAndDropVisualMode.Rejected;
            }
            
            if (!args.performDrop)
            {
                return DragAndDropVisualMode.Link;
            }

            foreach (var item in DragAndDrop.GetGenericData("Items") as List<int>)
            {
                var data = this.idToDataMap[item] as TransformData;

                // Unity gets mad if we swap parent and child, so don't allow that.
                if (data == (targetData as TransformData).Parent)
                {
                    return DragAndDropVisualMode.Rejected;
                }

                data.Parent = targetData as TransformData;

                var sceneProxy = this.getSceneProxy(data.DataSetGuid, data.Name);
                if (sceneProxy == null)
                {
                    continue;
                }

                var parentSceneProxy = this.getSceneProxy(data.Parent.DataSetGuid, data.Parent.Name);
                if (parentSceneProxy == null)
                {
                    continue;
                }

                sceneProxy.transform.SetParent(parentSceneProxy.transform);
            }

            this.Reload();
            DragAndDrop.AcceptDrag();
            return DragAndDropVisualMode.Link;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            // TODO handle multiple selections
            var selected = (from id in selectedIds select this.idToDataMap[id]).ToArray();
            
            if (selected.Length == 0)
            {
                SingletonScriptableObject<DataListWindowState>.Instance.InspectedEntity = null;
                //FoxKitEditor.InspectedEntity = null;
                return;
            }

            SingletonScriptableObject<DataListWindowState>.Instance.InspectedEntity = selected[0];

            // For each TransformData selected, select its scene proxy.
            var newSelection = new List<UnityEngine.Object>();
            foreach (var id in selectedIds)
            {
                var data = this.idToDataMap[id];
                var transformData = data as TransformData;

                if (data is DataSet)
                {
                    var dataSetAsset = AssetDatabase.LoadAssetAtPath<EntityFileAsset>(AssetDatabase.GUIDToAssetPath(data.DataSetGuid));
                    newSelection.Add(dataSetAsset);
                    continue;
                }
                else if (transformData == null)
                {
                    continue;
                }

                var sceneProxy = this.getSceneProxy(data.DataSetGuid, data.Name);
                if (sceneProxy != null)
                {
                    newSelection.Add(sceneProxy.gameObject);
                }
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
            if (clickedDataSet == null)
            {
                return;
            }

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
            var dataListWindowState = SingletonScriptableObject<DataListWindowState>.Instance;
            if (dataListWindowState.InspectedEntity == id as DataSet)
            {
                dataListWindowState.InspectedEntity = null;
            }
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