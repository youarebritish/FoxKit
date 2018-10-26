namespace FoxKit.Modules.DataSet.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.Editor.DataListWindow;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

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
        private List<Data> selectableEntities = new List<Data>();

        private Data selectedEntity;

        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField] TreeViewState treeViewState;

        //The TreeView is not serializable, so it should be reconstructed from the tree data.
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

        public static EditorWindow Create(Action<Data> onEntitySelectedCallback)
        {
            var window = GetWindow<SelectEntityWindow>(true, "Select Entity", true);
            window.onEntitySelected = onEntitySelectedCallback;
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

            var activeDataSetGuid = SingletonScriptableObject<DataListWindowState>.Instance.ActiveDataSetGuid;
            Assert.IsFalse(string.IsNullOrEmpty(activeDataSetGuid));

            var activeDataSet = AssetDatabase.LoadAssetAtPath<DataSetAsset>(AssetDatabase.GUIDToAssetPath(activeDataSetGuid));
            this.selectableEntities = activeDataSet.GetDataSet().dataList.Values.ToList();

            this.treeView = new SelectEntityWindowTreeView(this.treeViewState, this.selectableEntities, this.OnEntitySelected);
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
                // Remove focus if cleared
                this.searchString = string.Empty;
                GUI.FocusControl(null);
            }

            EditorGUILayout.EndHorizontal();

            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height));
            this.treeView.SetSearchString(this.searchString);
            this.treeView.OnGUI(new Rect(0, 0, this.position.width, this.position.height - 5));

            EditorGUILayout.EndScrollView();

            EditorGUILayout.LabelField("hello");
        }

        private void OnEntitySelected(Data entity)
        {
            this.selectedEntity = entity;
        }

        private void OnDestroy()
        {
            this.onEntitySelected(this.selectedEntity);
        }
    }
}