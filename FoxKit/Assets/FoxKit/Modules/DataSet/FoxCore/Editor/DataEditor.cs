using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(Data), true)]
    public class DataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            var prop = this.serializedObject.GetIterator();
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
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        const float kIconSize = 16;

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
    }
}