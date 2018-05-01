namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;
    using System.IO;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

    public class DataListWindow : EditorWindow
    {
        public static HashSet<DataSet> openDataSets = new HashSet<DataSet>();

        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField]
        private TreeViewState mTreeViewState;

        //The TreeView is not serializable, so it should be reconstructed from the tree data.
        private SimpleTreeView mSimpleTreeView;

        public static void OpenDataSet(DataSet dataSet)
        {
            Debug.Log("Opening " + dataSet.name);
            openDataSets.Add(dataSet);
        }

        [OnOpenAsset]
        static bool OnOpenedAsset(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
            var dataSet = asset as DataSet;
            if (dataSet == null)
            {
                return false;
            }
            OpenDataSet(dataSet);
            return true;
        }

        void OnEnable()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (this.mTreeViewState == null) this.mTreeViewState = new TreeViewState();

            this.mSimpleTreeView = new SimpleTreeView(this.mTreeViewState);
        }

        private bool collapsed;

        private bool clearOnPlay;

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Load", "ToolbarButton", GUILayout.Width(45f)))
            {
                EditorGUIUtility.ShowObjectPicker<DataSet>(null, false, string.Empty, 0);
            }
            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (openDataSets.Count > 0)
            {
                this.mSimpleTreeView.OnGUI(new Rect(0, 17, position.width, position.height));
            }
        }
        
        [MenuItem("FoxKit/Data List Window")]
        static void ShowWindow()
        {
            var window = GetWindow<DataListWindow>();
            window.titleContent = new GUIContent("Data List");
            window.Show();
        }
    }

    public class SimpleTreeView : TreeView
    {
        private readonly HashSet<DataSet> dataSets = new HashSet<DataSet>();

        public SimpleTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        public void AddDataSet(DataSet dataSet)
        {
            this.dataSets.Add(dataSet);
            this.Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            int index = 1;
            foreach (var dataSet in DataListWindow.openDataSets)
            {
                var root = new TreeViewItem { id = 0, depth = -1, displayName = dataSet.name };

                foreach (var data in dataSet.GetDataList())
                {
                    var child = new TreeViewItem { id = index, displayName = data.Key };
                    child.AddChild(new TreeViewItem { id = index + 1, displayName = "test" });
                    root.AddChild(child);

                    index += 2;
                }

                SetupDepthsFromParentsAndChildren(root);
                return root;
            }

            return new TreeViewItem { id = 0, depth = -1, displayName = "error" };
        }
    }
}