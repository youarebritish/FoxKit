using UnityEngine;

namespace FoxKit.Utils
{
    using System;
    
    using UnityEditor;

    /// <summary>
    /// Helper functions for FoxKit UI.
    /// </summary>
    public static class FoxKitUiUtils
    {
        /// <summary>
        /// Height and width of FoxKit buttons.
        /// </summary>
        public const float BUTTON_DIMENSION = 32.0f;

        /// <summary>
        /// Draw a FoxKit tool button.
        /// </summary>
        /// <param name="icon">Icon to display on the button.</param>
        /// <param name="tooltip">Tooltip text.</param>
        /// <returns>True if the button was pressed.</returns>
        public static bool ToolButton(Texture icon, string tooltip)
        {
            var content = new GUIContent(icon, tooltip);
            return GUILayout.Button(
                content,
                GUILayout.MaxWidth(BUTTON_DIMENSION),
                GUILayout.MaxHeight(BUTTON_DIMENSION));
        }

        public static sbyte SbyteField(string label, sbyte value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, sbyte.MinValue, sbyte.MaxValue);
            return (sbyte)newValue;
        }

        public static byte ByteField(string label, byte value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, byte.MinValue, byte.MaxValue);
            return (byte)newValue;
        }

        public static short ShortField(string label, short value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, short.MinValue, short.MaxValue);
            return (short)newValue;
        }

        public static ushort UShortField(string label, ushort value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, ushort.MinValue, ushort.MaxValue);
            return (ushort)newValue;
        }

        public static uint UIntField(string label, uint value)
        {
            var newValue = EditorGUILayout.LongField(label, value);
            newValue = MathUtils.Clamp(newValue, uint.MinValue, uint.MaxValue);
            return Convert.ToUInt32(newValue);
        }

        public static ulong ULongField(string label, ulong value)
        {
            var newValue = EditorGUILayout.TextField(label, value.ToString());
            ulong parseResult;
            return ulong.TryParse(newValue, out parseResult) ? parseResult : value;
        }

        public static Quaternion QuaternionField(string label, Quaternion value)
        {
            var euler = value.eulerAngles;
            var rawValue = EditorGUILayout.Vector3Field(label, euler);
            return Quaternion.Euler(rawValue);
        }

        public static object EntityPtrField(string label, object value, Type type, bool allowSceneObjects = false)
        {
            // TODO Icon
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label));
            
            if (value == null)
            {
                if (GUILayout.Button($"Create {type.Name}", EditorStyles.miniButton))
                {
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField(new GUIContent(value.GetType().Name), EditorStyles.objectField);
                GUI.enabled = true;
            }
            
            EditorGUILayout.EndHorizontal();

            return value;
        }

        public static object EntityHandleField(string label, object value, Type type, bool allowSceneObjects = false)
        {
            // TODO Icon
            // TODO Picker
            // TODO Select on click
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label));

            var text = value?.GetType().Name ?? $"Null ({type.Name})";
            EditorGUILayout.LabelField(new GUIContent(text), EditorStyles.objectField);

            EditorGUILayout.EndHorizontal();

            return value;
        }
    }
}