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
            public GUIStyle componentButton = new GUIStyle((GUIStyle)"PR Label");

            /*public Styles()
            {
                this.componentButton.alignment = TextAnchor.MiddleLeft;
                this.componentButton.padding.left -= 5;
                this.componentButton.fixedHeight = 20f;
            }*/
        }

        public static SearchableEditorWindow Create()
        {
            var window = GetWindow<AddEntityWindow>();//true);
            window.titleContent = new GUIContent("Add Entity");
            
            window.minSize = new Vector2(230, 320);
            window.Show();
            return window;
        }

        static Styles _styles = new Styles();

        void OnGUI()
        {
            this.searchString = EditorGUILayout.TextField(this.searchString);

            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, GUILayout.Width(this.position.width), GUILayout.Height(this.position.height + 10));

            foreach (var type in DataSetImporter.EntityTypes)
            {
                if (type.Name.IndexOf(this.searchString, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    var buttonRect = EditorGUILayout.GetControlRect(true, 20f, _styles.componentButton);
                    if (GUI.Button(buttonRect, type.Name, _styles.componentButton))
                    {
                        DataListWindow.GetInstance().AddEntity(type);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}