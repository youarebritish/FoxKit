namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Linq;

    using FoxKit.Modules.Archive;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class SelectEntityWindow : EditorWindow
    {
        private string searchString = string.Empty;
        private Vector2 scrollPos;

        private Type baseClass;
        private Action<Data> onEntitySelected;

        private Action<DataIdentifier, string> onDataIdentifierEntitySelected;

        private Data selectedEntity;

        private bool wasDataIdentifierSelected;

        private DataIdentifier selectedDataIdentifier;

        private string selectedDataIdentifierKey;

        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField] TreeViewState treeViewState;

        // The TreeView is not serializable, so it should be reconstructed from the tree data.
        SelectEntityWindowTreeView treeView;

        private class Styles
        {
            public GUIStyle componentButton = new GUIStyle("PR Label");

            public Styles()
            {
                this.componentButton.alignment = TextAnchor.MiddleLeft;
                this.componentButton.padding.left -= 5;
                this.componentButton.fixedHeight = 20f;
            }
        }

        public static EditorWindow Create(Action<Data> onEntitySelectedCallback, Action<DataIdentifier, string> onDataIdentifierEntitySelected)
        {
            var window = GetWindow<SelectEntityWindow>(true, "Select Entity", true);
            window.onEntitySelected = onEntitySelectedCallback;
            window.onDataIdentifierEntitySelected = onDataIdentifierEntitySelected;
            window.minSize = new Vector2(230, 320);
            window.ShowAuxWindow();
            return window;
        }

        static SelectEntityWindow.Styles _styles;

        private void OnEnable()
        {
            if (this.treeViewState == null)
            {
                this.treeViewState = new TreeViewState();
            }
            
            var packages = (from guid in AssetDatabase.FindAssets($"t:{typeof(PackageDefinition).Name}")
                            let package = AssetDatabase.LoadAssetAtPath<PackageDefinition>(AssetDatabase.GUIDToAssetPath(guid))
                            where package.Type == PackageDefinition.PackageType.Fpkd
                            select package).ToList();

            this.treeView = new SelectEntityWindowTreeView(this.treeViewState, packages, this.OnEntitySelected, this.OnDataIdentifierEntitySelected);
        }

        private void OnGUI()
        {
            if (_styles == null)
            {
                _styles = new SelectEntityWindow.Styles();
            }

            // Search bar.
            EditorGUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            this.searchString = EditorGUILayout.TextField(this.searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            if (GUILayout.Button(string.Empty, GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared.
                this.searchString = string.Empty;
                GUI.FocusControl(null);
            }

            EditorGUILayout.EndHorizontal();

            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height));
            this.treeView.SetSearchString(this.searchString);
            this.treeView.OnGUI(new Rect(0, 0, this.position.width, this.position.height - 5));

            EditorGUILayout.EndScrollView();
        }

        private void OnEntitySelected(Data entity)
        {
            this.selectedEntity = entity;
            this.selectedDataIdentifier = null;
            this.selectedDataIdentifierKey = null;
            this.wasDataIdentifierSelected = false;
        }

        private void OnDataIdentifierEntitySelected(DataIdentifier dataIdentifier, string key)
        {
            Assert.IsNotNull(dataIdentifier);
            Assert.IsFalse(string.IsNullOrEmpty(key));
            Assert.IsNotNull(dataIdentifier.Links);
            Assert.IsTrue(dataIdentifier.Links.ContainsKey(key));

            this.selectedEntity = null;
            this.selectedDataIdentifier = dataIdentifier;
            this.selectedDataIdentifierKey = key;
            this.wasDataIdentifierSelected = true;
        }

        private void OnDestroy()
        {
            if (this.wasDataIdentifierSelected)
            {
                Assert.IsNotNull(this.selectedDataIdentifier);
                Assert.IsNotNull(this.selectedDataIdentifierKey);

                this.onDataIdentifierEntitySelected(this.selectedDataIdentifier, this.selectedDataIdentifierKey);
                return;
            }

            this.onEntitySelected(this.selectedEntity);
        }
    }
}