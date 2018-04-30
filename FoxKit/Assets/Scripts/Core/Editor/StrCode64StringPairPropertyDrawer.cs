namespace FoxKit.Core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(StrCode64StringPair))]
    public class StrCode64StringPairPropertyDrawer : PropertyDrawer
    {
        public string[] options = { "Hash", "String" };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            var labelPosition = position;

            EditorGUI.LabelField(position, property.name);

            var popupPosition = labelPosition;
            popupPosition.width = Screen.width / 7;
            popupPosition.x = position.x + Screen.width / 1.35f;

            property.FindPropertyRelative("_isUnhashed").boolValue = System.Convert.ToBoolean(EditorGUI.Popup(popupPosition, System.Convert.ToInt32(property.FindPropertyRelative("_isUnhashed").boolValue), options));

            var fieldPosition = labelPosition;
            fieldPosition.width = Screen.width / 2f;
            fieldPosition.x = position.x + Screen.width / 3.5f;

            if (property.FindPropertyRelative("_isUnhashed").boolValue == true)
            {
                EditorGUI.PropertyField(fieldPosition, property.FindPropertyRelative("_string"), GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(fieldPosition, property.FindPropertyRelative("_hash"), GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
	}
}