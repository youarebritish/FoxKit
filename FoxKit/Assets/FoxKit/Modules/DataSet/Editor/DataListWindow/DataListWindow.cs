namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System.Collections.Generic;

    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;

    public class DataListWindow : EditorWindow
    {
        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField]
        private TreeViewState mTreeViewState;

        //The TreeView is not serializable, so it should be reconstructed from the tree data.
        private SimpleTreeView mSimpleTreeView;

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
                Debug.Log("You click Clear button");
            }
            GUILayout.Space(5f);
            /*
            // Create toggles button.
            collapsed = GUILayout.Toggle(collapsed, "Collapse", "ToolbarButton");
            clearOnPlay = GUILayout.Toggle(clearOnPlay, "Clear on Play", "ToolbarButton");*/
            // Push content to be what they should be. (ex. width)
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            this.mSimpleTreeView.OnGUI(new Rect(0, 17, position.width, position.height));
        }

        // Add menu named "My Window" to the Window menu
        [MenuItem("FoxKit/Data List Window")]
        static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<DataListWindow>();
            window.titleContent = new GUIContent("Data List");
            window.Show();
        }
    }

    public class SimpleTreeView : TreeView
    {
        public SimpleTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            // This section illustrates that IDs should be unique. The root item is required to 
            // have a depth of -1, and the rest of the items increment from that.
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var allItems = new List<TreeViewItem>
                               {
                                   new TreeViewItem {id = 1, depth = 0, displayName = "Animals"},
                                   new TreeViewItem {id = 2, depth = 1, displayName = "Mammals"},
                                   new TreeViewItem {id = 3, depth = 2, displayName = "Tiger"},
                                   new TreeViewItem {id = 4, depth = 2, displayName = "Elephant"},
                                   new TreeViewItem {id = 5, depth = 2, displayName = "Okapi"},
                                   new TreeViewItem {id = 6, depth = 2, displayName = "Armadillo"},
                                   new TreeViewItem {id = 7, depth = 1, displayName = "Reptiles"},
                                   new TreeViewItem {id = 8, depth = 2, displayName = "Crocodile"},
                                   new TreeViewItem {id = 9, depth = 2, displayName = "Lizard"},
                               };

            // Utility method that initializes the TreeViewItem.children and .parent for all items.
            SetupParentsAndChildrenFromDepths(root, allItems);

            // Return root of the tree
            return root;
        }
    }
}