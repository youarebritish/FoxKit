using UnityEditor;

namespace FoxKit.Modules.DataSet.FoxCore.Editor
{
    using UnityEngine;

    [CustomEditor(typeof(Data), true)]
    public class DataEditor : EntityEditor
    {
        private const float iconSize = 16;
        
        protected override void OnHeaderGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.FindStyle("In BigTitle"));

            EditorGUILayout.BeginHorizontal();
            /*GUI.DrawTexture(
                GUILayoutUtility.GetRect(iconSize, iconSize, GUILayout.ExpandWidth(false)),
                (this.target as Data).Icon);*/
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