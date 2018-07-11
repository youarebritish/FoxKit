namespace FoxKit.Modules.DataSet.Editor.DataListWindow
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.Importer;

    using UnityEditor;

    using UnityEngine;

    public class AddEntityWindow : SearchableEditorWindow
    {
        private string searchString = string.Empty;
        private Vector2 scrollPos;
        
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

        public static SearchableEditorWindow Create()
        {
            var window = GetWindow<AddEntityWindow>();
            window.titleContent = new GUIContent("New Entity");
            
            window.minSize = new Vector2(230, 320);
            window.Show();
            return window;
        }

        static Styles _styles;

        void OnGUI()
        {
            if (_styles == null)
            {
                _styles = new Styles();
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
            
            foreach (var type in DataSetImporter.EntityTypes)
            {
                if (type.IsSubclassOf(typeof(DataElement)) || type == typeof(DataSet))
                {
                    continue;
                }

                // Filter out results that don't contain search string.
                if (type.Name.IndexOf(this.searchString, 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                {
                    continue;
                }

                var buttonRect = EditorGUILayout.GetControlRect(true, 20f, _styles.componentButton);
                if (!GUI.Button(buttonRect, type.Name, _styles.componentButton))
                {
                    continue;
                }
                if (DataListWindow.GetInstance().ActiveDataSet == null)
                {
                    Debug.LogError("Can't create an Entity without an active DataSet.");
                    return;
                }

                DataListWindow.GetInstance().AddEntity(type);
            }

            GUI.enabled = true;
            EditorGUILayout.EndScrollView();
        }
    }
}