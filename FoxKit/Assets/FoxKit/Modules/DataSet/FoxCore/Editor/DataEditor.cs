using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UnityEngine;

    [CustomEditor(typeof(Data), true)]
    public class DataEditor : UnityEditor.Editor
    {
        const float kIconSize = 16;

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            /*var prop = this.serializedObject.GetIterator();
            if (prop.NextVisible(true))
            {
                do
                {
                    // We don't care about Unity's default "script" field.
                    if (prop.name == "m_Script")
                    {
                        continue;
                    }
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty(prop.name), true);
                }
                while (prop.NextVisible(false));
            }*/

            this.DoCategorizedFields();

            this.serializedObject.ApplyModifiedProperties();
        }

        protected override void OnHeaderGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.FindStyle("In BigTitle"));

            EditorGUILayout.BeginHorizontal();
            GUI.DrawTexture(
                GUILayoutUtility.GetRect(kIconSize, kIconSize, GUILayout.ExpandWidth(false)),
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

        private void DoCategorizedFields()
        {
            var fieldGroups = GetFieldsSortedByCategory(this.target);

            foreach (var fieldGroup in fieldGroups)
            {
                //EditorGUILayout.Foldout(foldouts[fieldGroup.Key], fieldGroup.Key);
                EditorGUILayout.Foldout(true, fieldGroup.Key);
                //EditorGUILayout.LabelField(fieldGroup.Key);

                foreach (var field in fieldGroup)
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name), true);
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
                   select category;
        }
    }
}