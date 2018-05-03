namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

    public class DataListWindow : EditorWindow
    {
        /// <summary>
        /// DataSets currently open in the window.
        /// </summary>
        [SerializeField]
        private readonly List<DataSet> openDataSets = new List<DataSet>();

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

            this.simpleTreeView = new SimpleTreeView(this.treeViewState, this.openDataSets);
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
                this.simpleTreeView.OnGUI(new Rect(0, 17, position.width, position.height - 17));
            }
        }

    }

    public class SimpleTreeView : TreeView
    {
        private readonly Dictionary<int, Data> idToDataMap = new Dictionary<int, Data>();

        [SerializeField]
        private readonly List<DataSet> openDataSets;

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
            foreach (var dataSet in openDataSets)
            {
                var dataSetNode = new TreeViewItem { id = index, displayName = dataSet.name };
                this.idToDataMap.Add(index, dataSet);
                root.AddChild(dataSetNode);
                index++;

                foreach (var data in dataSet.GetDataList())
                {
                    var child = new TreeViewItem { id = index, displayName = data.Key };
                    dataSetNode.AddChild(child);
                    this.idToDataMap.Add(index, data.Value);
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
    }
}