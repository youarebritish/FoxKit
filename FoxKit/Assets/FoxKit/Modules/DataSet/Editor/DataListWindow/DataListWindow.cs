namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

    public class DataListWindow : EditorWindow
    {
        private const string PreferenceKeyOpenDataSets = "FoxKit.DataListWindow.OpenDataSets";

        /// <summary>
        /// DataSets currently open in the window.
        /// </summary>
        [SerializeField]
        private List<DataSet> openDataSets;

        /// <summary>
        /// Serializable state of the TreeView.
        /// </summary>
        [SerializeField]
        private TreeViewState treeViewState;

        /// <summary>
        /// Tree view widget.
        /// </summary>
        private SimpleTreeView simpleTreeView;

        /// <summary>
        /// Called when the user double clicks on an asset.
        /// Checks if the asset is a DataSet, and if so, opens it in the Data List Window and gives it focus.
        /// </summary>
        /// <param name="instanceId">
        /// The instance ID of the selected asset.
        /// </param>
        /// <param name="line">
        /// The line number.
        /// </param>
        /// <returns>
        /// True if the asset's opening was handled by the Data List Window, else false.
        /// </returns>
        [OnOpenAsset]
        private static bool OnOpenedAsset(int instanceId, int line = -1)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceId) as DataSet;

            if (asset == null)
            {
                return false;
            }

            var window = MakeOrGetWindow();
            window.OpenDataSet(asset);
            window.Focus();
            return true;
        }

        private static void SaveOpenDataSets(IEnumerable<DataSet> openDataSets)
        {
            var openDataSetsPaths = from dataSet in openDataSets
                                    select AssetDatabase.GetAssetPath(dataSet);
            PlayerPrefsX.SetStringArray(PreferenceKeyOpenDataSets, openDataSetsPaths.ToArray());
        }

        private static IEnumerable<DataSet> GetLastOpenDataSets()
        {
            var lastOpenDataSetsPaths = PlayerPrefsX.GetStringArray(PreferenceKeyOpenDataSets);
            return from path in lastOpenDataSetsPaths
                   where !string.IsNullOrEmpty(path)
                   select AssetDatabase.LoadAssetAtPath<DataSet>(path);
        }

        /// <summary>
        /// Gets the current Data List Window or makes a new instance if it's not currently open.
        /// </summary>
        /// <returns>
        /// The <see cref="DataListWindow"/>.
        /// </returns>
        [MenuItem("FoxKit/Data List Window")]
        private static DataListWindow MakeOrGetWindow()
        {
            var window = GetWindow<DataListWindow>();
            window.titleContent = new GUIContent("Data List");
            window.Show();
            return window;
        }

        /// <summary>
        /// When the window is loaded, initialize the TreeView.
        /// </summary>
        private void OnEnable()
        {
            if (this.treeViewState == null)
            {
                this.treeViewState = new TreeViewState();
            }

            if (this.openDataSets == null)
            {
                this.openDataSets = new List<DataSet>();
            }

            this.openDataSets = GetLastOpenDataSets().ToList();
            this.simpleTreeView = new SimpleTreeView(this.treeViewState, this.openDataSets);
            this.simpleTreeView.Reload();
        }

        private void OnDisable()
        {
            SaveOpenDataSets(this.openDataSets);
        }

        /// <summary>
        /// Opens a DataSet in the Data List Window.
        /// </summary>
        /// <param name="dataSet">
        /// The DataSet to open.
        /// </param>
        private void OpenDataSet(DataSet dataSet)
        {
            if (this.openDataSets.Contains(dataSet))
            {
                return;
            }

            this.openDataSets.Add(dataSet);
            this.simpleTreeView.Reload();
        }

        /// <summary>
        /// Update and draw the window's UI.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Load", "ToolbarButton", GUILayout.Width(45f)))
            {
                EditorGUIUtility.ShowObjectPicker<DataSet>(null, false, string.Empty, 0);
            }

            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (this.openDataSets.Count > 0)
            {
                this.simpleTreeView.OnGUI(new Rect(0, 17, this.position.width, this.position.height - 17));
            }
        }
    }

    public static class DataListWindowItemContextMenuFactory
    {
        public delegate void ShowContextMenuDelegate();

        public static ShowContextMenuDelegate Create()
        {
            return ShowContextMenu;
        }

        private static void ShowContextMenu()
        {
            var menu = new GenericMenu();
            AddMenuItem(menu, "Set Active DataSet", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Save DataSet", OnSetActiveDataSet);
            AddMenuItem(menu, "Save DataSet As", OnSetActiveDataSet);
            AddMenuItem(menu, "Save All", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Unload DataSet", OnSetActiveDataSet);
            AddMenuItem(menu, "Remove DataSet", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Discard changes", OnSetActiveDataSet);

            menu.AddSeparator(string.Empty);

            AddMenuItem(menu, "Select DataSet Asset", OnSetActiveDataSet);
            AddMenuItem(menu, "Add Entity", OnSetActiveDataSet);

            menu.ShowAsContext();
        }

        private static void AddMenuItem(GenericMenu menu, string text, GenericMenu.MenuFunction callback)
        {
            menu.AddItem(new GUIContent(text), false, callback);
        }

        private static void OnSetActiveDataSet()
        {
            // TODO
        }
    }

    public class SimpleTreeView : TreeView
    {
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

        public SimpleTreeView(TreeViewState treeViewState, List<DataSet> openDataSets)
            : base(treeViewState)
        {
            this.showAlternatingRowBackgrounds = true;
            this.openDataSets = openDataSets;
        }
        
        protected override TreeViewItem BuildRoot()
        {
            this.idToDataMap.Clear();

            var index = 1;
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "root" };

            // ID 0 is the root, not an actual Data reference.
            this.idToDataMap.Add(null);

            foreach (var dataSet in this.openDataSets)
            {
                var dataSetNode = new TreeViewItem { id = index, displayName = dataSet.name };
                this.idToDataMap.Add(dataSet);
                this.dataSetTreeIds.Add(index);
                root.AddChild(dataSetNode);
                index++;

                foreach (var data in dataSet.GetDataList())
                {
                    var child = new TreeViewItem { id = index, displayName = data.Key };
                    dataSetNode.AddChild(child);
                    this.idToDataMap.Add(data.Value);
                    index++;
                }

                TreeView.SetupDepthsFromParentsAndChildren(root);
            }

            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            Selection.objects = (from id in selectedIds select this.idToDataMap[id]).ToArray();
        }

        protected override void ContextClickedItem(int id)
        {
            if (this.dataSetTreeIds.Contains(id))
            {
                DataListWindowItemContextMenuFactory.Create()();
            }
        }
    }
}