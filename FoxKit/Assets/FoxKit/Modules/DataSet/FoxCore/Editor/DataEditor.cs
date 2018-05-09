using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    [CustomEditor(typeof(Data), true)]
    public class DataEditor : UnityEditor.Editor
    {
        private const float iconSize = 16;

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUIUtility.labelWidth = 200;

            this.DoCategorizedFields();

            this.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnHeaderGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.FindStyle("In BigTitle"));

            EditorGUILayout.BeginHorizontal();
            GUI.DrawTexture(
                GUILayoutUtility.GetRect(iconSize, iconSize, GUILayout.ExpandWidth(false)),
                (this.target as Data).Icon);
            var style = new GUIStyle(GUI.skin.label)
                            {
                                alignment = TextAnchor.MiddleLeft,
                                fontStyle = FontStyle.Normal,
                                padding = new RectOffset(0, 0, 0, 5)
                            };
            EditorGUILayout.LabelField(this.target.GetType().Name, style);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
        
        private readonly Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        private void DoCategorizedFields()
        {
            var fieldGroups = GetFieldsSortedByCategory(this.target);

            foreach (var fieldGroup in fieldGroups)
            {
                var wasUnfolded = true;
                var wasUnfoldedValueFound = this.foldouts.TryGetValue(fieldGroup.Key, out wasUnfolded);
                var unfolded = DataEditorUI.Foldout(fieldGroup.Key, wasUnfolded);

                if (wasUnfoldedValueFound)
                {
                    this.foldouts[fieldGroup.Key] = unfolded;
                }
                else
                {
                    this.foldouts.Add(fieldGroup.Key, unfolded);
                }

                if (!unfolded)
                {
                    continue;
                }

                foreach (var field in fieldGroup)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        var serializedProperty = this.serializedObject.FindProperty(field.Name);
                        if (field.GetCustomAttribute<CategoryAttribute>().ShowNestedInspector)
                        {
                            var nestedEditor = CreateEditor(serializedProperty.objectReferenceValue);
                            nestedEditor.DrawDefaultInspector();
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedProperty, true);
                        }
                    }
                }
            }
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