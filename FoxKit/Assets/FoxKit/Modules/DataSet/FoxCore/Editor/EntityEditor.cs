using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(Entity), true)]
    public class EntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUIUtility.labelWidth = 200;

            this.DoCategorizedFields();

            this.serializedObject.ApplyModifiedProperties();
        }
        
        [SerializeField]
        private List<string> unfoldedFields = new List<string>();

        [SerializeField]
        private List<NestedEditorEntry> nestedEditors = new List<NestedEditorEntry>();

        [Serializable]
        private class NestedEditorEntry
        {
            public string Name;
            public Editor Editor;

            public NestedEditorEntry(string name, Editor editor)
            {
                this.Name = name;
                this.Editor = editor;
            }
        }
        
        private void DoCategorizedFields()
        {
            if (this.unfoldedFields == null)
            {
                Debug.Log("nulled in " + target.GetType().ToString());
                this.unfoldedFields = new List<string>();
            }

            var fieldGroups = GetFieldsSortedByCategory(this.target);
            var newUnfoldedEntries = new List<string>();

            foreach (var fieldGroup in fieldGroups)
            {
                var wasUnfolded = this.unfoldedFields.FirstOrDefault(entry => entry == fieldGroup.Key);
                var unfolded = DataEditorUI.Foldout(fieldGroup.Key, !string.IsNullOrEmpty(wasUnfolded));
                
                if (!unfolded)
                {
                    continue;
                }

                newUnfoldedEntries.Add(fieldGroup.Key);

                foreach (var field in fieldGroup)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        var serializedProperty = this.serializedObject.FindProperty(field.Name);
                        if (field.GetCustomAttribute<CategoryAttribute>().ShowNestedInspector == CategoryAttribute.NestedInspectorMode.Draw)
                        {
                            Editor nestedEditor;
                            var nestedEditorEntry = this.nestedEditors.FirstOrDefault(entry => entry.Name == field.Name);
                            if (nestedEditorEntry == null)
                            {
                                nestedEditor = CreateEditor(serializedProperty.objectReferenceValue, typeof(EntityEditor));
                                this.nestedEditors.Add(new NestedEditorEntry(field.Name, nestedEditor));
                            }
                            else
                            {
                                nestedEditor = nestedEditorEntry.Editor;
                            }
                            
                            nestedEditor.OnInspectorGUI();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedProperty, true);
                        }
                    }
                }
            }

            this.unfoldedFields = newUnfoldedEntries;
        }

        private static IEnumerable<FieldInfo> GetCategorizedFields(object obj)
        {
            return from field in obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                   where field.IsDefined(typeof(CategoryAttribute), true)
                   select field;
        }

        private static IEnumerable<IGrouping<string, FieldInfo>> GetFieldsSortedByCategory(object obj)
        {
            return from field in GetCategorizedFields(obj)
                   group field by field.GetCustomAttribute<CategoryAttribute>().Category
                   into category
                   orderby category.Key
                   select category;
        }
    }

    public static class DataEditorUI
    {
        public static bool Foldout(string title, bool display)
        {
            var style = new GUIStyle("ShurikenModuleTitle")
                            {
                                font = new GUIStyle(EditorStyles.label).font,
                                border = new RectOffset(15, 7, 4, 4),
                                fixedHeight = 22,
                                contentOffset = new Vector2(20f, -2f)
                            };

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            rect = EditorGUI.IndentedRect(rect);

            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
        }
    }
}