namespace FoxKit.Core
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(StrCode32StringPair))]
    public class StrCode32StringPairPropertyDrawer : PropertyDrawer
    {
        public string[] options = { "Hash", "String" };
        public int index = 0;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            var isStringOrHash = System.Convert.ToInt32(property.FindPropertyRelative("_isUnhashed").boolValue);
            var popupRect = new Rect(position.x * 15, position.y, position.width / 4, position.height);
            isStringOrHash = EditorGUI.Popup(popupRect, isStringOrHash, options);

            var rectPosition = position;
            rectPosition.width = position.width * .333f;
            rectPosition.x = position.x * 4.5f;

            if (isStringOrHash == 1)
            {
                EditorGUI.PropertyField(rectPosition, property.FindPropertyRelative("_string"), GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(rectPosition, property.FindPropertyRelative("_hash"), GUIContent.none);
            }

            EditorGUI.EndProperty();
        }
    }
}