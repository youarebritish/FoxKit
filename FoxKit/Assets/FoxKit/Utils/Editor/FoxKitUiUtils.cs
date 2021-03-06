﻿using UnityEngine;

namespace FoxKit.Utils
{
    using System;

    using FoxKit.Core;
    using FoxKit.Modules.DataSet.Editor;
    using FoxKit.Modules.DataSet.Editor.DataListWindow;
    using FoxKit.Modules.DataSet.Fox.FoxCore;
    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEditor;

    using UnityEngine.Assertions;

    using Object = UnityEngine.Object;

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
        /// Display the read-only asset warning and provide the user with a button to create an editable copy of the asset.
        /// </summary>
        /// <typeparam name="T">Type of the asset.</typeparam>
        /// <param name="asset">The original, read-only asset.</param>
        /// <param name="setNotReadOnly">Function to set an asset of type T to not read-only.</param>
        public static void ReadOnlyWarningAndButton<T>(T asset, Action<T> setNotReadOnly) where T : UnityEngine.Object
        {
            GUI.enabled = true;
            EditorGUILayout.HelpBox(
                "Unity marks imported assets as read-only. To make changes to this asset, create an editable copy.",
                MessageType.Warning);
            if (GUILayout.Button("Create Editable Copy", GUILayout.Width(200)))
            {
                var duplicate = Object.Instantiate(asset);
                setNotReadOnly(duplicate);

                var path = EditorUtility.SaveFilePanelInProject(
                    "Create editable copy",
                    $"{asset.name}_copy",
                    "asset",
                    "Create editable copy");

                if (!string.IsNullOrEmpty(path))
                {
                    EditorUtility.SetDirty(duplicate);
                    AssetDatabase.CreateAsset(duplicate, path);
                    AssetDatabase.SaveAssets();
                }
            }

            EditorGUILayout.Separator();
            GUI.enabled = false;
        }

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

        public static sbyte SbyteField(Rect position, sbyte value)
        {
            var newValue = EditorGUI.IntField(position, value);
            newValue = MathUtils.Clamp(newValue, sbyte.MinValue, sbyte.MaxValue);
            return (sbyte)newValue;
        }

        public static byte ByteField(string label, byte value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, byte.MinValue, byte.MaxValue);
            return (byte)newValue;
        }

        public static byte ByteField(Rect position, byte value)
        {
            var newValue = EditorGUI.IntField(position, value);
            newValue = MathUtils.Clamp(newValue, byte.MinValue, byte.MaxValue);
            return (byte)newValue;
        }

        public static short ShortField(string label, short value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, short.MinValue, short.MaxValue);
            return (short)newValue;
        }

        public static short ShortField(Rect position, short value)
        {
            var newValue = EditorGUI.IntField(position, value);
            newValue = MathUtils.Clamp(newValue, short.MinValue, short.MaxValue);
            return (short)newValue;
        }

        public static ushort UShortField(string label, ushort value)
        {
            var newValue = EditorGUILayout.IntField(label, value);
            newValue = MathUtils.Clamp(newValue, ushort.MinValue, ushort.MaxValue);
            return (ushort)newValue;
        }

        public static ushort UShortField(Rect position, ushort value)
        {
            var newValue = EditorGUI.IntField(position, value);
            newValue = MathUtils.Clamp(newValue, ushort.MinValue, ushort.MaxValue);
            return (ushort)newValue;
        }

        public static uint UIntField(string label, uint value)
        {
            var newValue = EditorGUILayout.LongField(label, value);
            newValue = MathUtils.Clamp(newValue, uint.MinValue, uint.MaxValue);
            return Convert.ToUInt32(newValue);
        }

        public static uint UIntField(Rect position, uint value)
        {
            var newValue = EditorGUI.LongField(position, value);
            newValue = MathUtils.Clamp(newValue, uint.MinValue, uint.MaxValue);
            return Convert.ToUInt32(newValue);
        }

        public static ulong ULongField(string label, ulong value)
        {
            var newValue = EditorGUILayout.TextField(label, value.ToString());
            ulong parseResult;
            return ulong.TryParse(newValue, out parseResult) ? parseResult : value;
        }

        public static ulong ULongField(Rect position, ulong value)
        {
            var newValue = EditorGUI.TextField(position, value.ToString());
            ulong parseResult;
            return ulong.TryParse(newValue, out parseResult) ? parseResult : value;
        }

        public static Quaternion QuaternionField(string label, Quaternion value)
        {
            var euler = value.eulerAngles;
            var rawValue = EditorGUILayout.Vector3Field(label, euler);
            return Quaternion.Euler(rawValue);
        }

        public static Quaternion QuaternionField(Rect position, Quaternion value)
        {
            var euler = value.eulerAngles;
            var rawValue = EditorGUI.Vector3Field(position, string.Empty, euler);
            return Quaternion.Euler(rawValue);
        }
        
        public static object EntityPtrField(string label, object value, Type type, Action createNewEntityCallback, Action<Entity> deleteEntityCallback, bool allowSceneObjects = false)
        {
            Assert.IsNotNull(type);
            Assert.IsNotNull(createNewEntityCallback);
            Assert.IsNotNull(deleteEntityCallback);

            // TODO Icon
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label));
            
            if (value == null)
            {
                if (GUILayout.Button($"Create {type.Name}", EditorStyles.miniButton))
                {
                    createNewEntityCallback();
                }
            }
            else
            {
                var isGuiEnabled = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.LabelField(new GUIContent(value.GetType().Name), EditorStyles.objectField);
                GUI.enabled = isGuiEnabled;

                if (GUILayout.Button("Delete", EditorStyles.miniButton))
                {
                    deleteEntityCallback(value as Entity);
                    return null;
                }
            }
            
            EditorGUILayout.EndHorizontal();

            return value;
        }
        
        public static object EntityPtrField(Rect position, object value, Type type, Action createNewEntityCallback, bool allowSceneObjects = false)
        {
            Assert.IsNotNull(type);
            Assert.IsNotNull(createNewEntityCallback);

            if (value == null)
            {
                if (GUI.Button(position, "Null", EditorStyles.miniButton))
                {
                    createNewEntityCallback();
                }
            }
            else
            {
                if (GUI.Button(position, $"Edit ({value.GetType().Name})", EditorStyles.miniButton))
                {
                    SingletonScriptableObject<DataListWindowState>.Instance.InspectedEntity = value as Entity;
                }
            }
            
            return value;
        }

        public static object EntityHandleField(string label, object value, Type type, bool allowSceneObjects = false)
        {
            // TODO Icon
            // TODO Picker
            // TODO Select on click
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent(label));

            var text = $"Null ({type.Name})";
            if (value != null)
            {
                var data = value as Data;
                text = data != null ? data.Name : value.ToString();
            }

            EditorGUILayout.LabelField(new GUIContent(text), EditorStyles.objectField);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        public static object EntityHandleField(Rect position, object value, Type type, bool allowSceneObjects = false)
        {
            // TODO Icon
            // TODO Picker
            // TODO Select on click
            EditorGUILayout.BeginHorizontal();

            var text = $"Null ({type.Name})";
            if (value != null)
            {
                var data = value as Data;
                text = data != null ? data.Name : value.ToString();
            }

            EditorGUI.LabelField(position, new GUIContent(text), EditorStyles.objectField);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        public static EntityLink EntityLinkField(string label, EntityLink value, Action<Data> entitySelectedCallback, Action<DataIdentifier, string> onDataIdentifierEntitySelectedCallback)
        {
            if (value == null)
            {
                value = new EntityLink();
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);

            // TODO Icon
            var labelText = "None (EntityLink)";
            if (value.IsDataIdentifierEntityLink)
            {
                labelText = $"{value.NameInArchive} (EntityLink)";
            }
            else if (value.Entity != null)
            {
                labelText = $"{value.Entity.Name} (EntityLink)";
            }

            var textFieldStyle = EditorStyles.textField;
            textFieldStyle.clipping = TextClipping.Clip;
            if (GUILayout.Button(labelText, textFieldStyle, GUILayout.ExpandWidth(true), GUILayout.MinWidth(0)))
            {
                if (value.IsDataIdentifierEntityLink)
                {
                    if (value.DataIdentifier != null)
                    {
                        //DataListWindow.GetInstance().OpenDataSet(value.DataIdentifier.DataSetGuid, value.DataIdentifier.Name);
                    }
                }
                else
                {
                    if (value.Entity != null)
                    {
                        DataListWindow.GetInstance().OpenDataSet(value.Entity.DataSetGuid, value.Entity.Name);
                    }
                }
            }

            var editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            if (GUILayout.Button(string.Empty, editorSkin.GetStyle("IN ObjectField"), GUILayout.Width(14f)))
            {
                SelectEntityWindow.Create(entitySelectedCallback, onDataIdentifierEntitySelectedCallback);
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3);
            return value;
        }

        public static EntityLink EntityLinkField(Rect rect, EntityLink value, Action<Data> entitySelectedCallback, Action<DataIdentifier, string> onDataIdentifierEntitySelectedCallback)
        {
            if (value == null)
            {
                value = new EntityLink();
            }
            
            // TODO Icon
            var labelText = "None (EntityLink)";
            if (value.IsDataIdentifierEntityLink)
            {
                labelText = $"{value.NameInArchive} (EntityLink)";
            }
            else if (value.Entity != null)
            {
                labelText = $"{value.Entity.Name} (EntityLink)";
            }

            var textFieldStyle = EditorStyles.textField;
            textFieldStyle.clipping = TextClipping.Clip;

            var mainButtonRect = rect;
            mainButtonRect.width -= 18;
            if (GUI.Button(mainButtonRect, labelText, textFieldStyle))
            {
                if (value.IsDataIdentifierEntityLink)
                {
                    if (value.DataIdentifier != null)
                    {
                        //DataListWindow.GetInstance().OpenDataSet(value.DataIdentifier.DataSetGuid, value.DataIdentifier.Name);
                    }
                }
                else
                {
                    if (value.Entity != null)
                    {
                        DataListWindow.GetInstance().OpenDataSet(value.Entity.DataSetGuid, value.Entity.Name);
                    }
                }
            }

            var editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            var openButtonRect = rect;
            openButtonRect.width = 14;
            openButtonRect.position = new Vector2(
                mainButtonRect.position.x + mainButtonRect.width + 4,
                mainButtonRect.position.y);
            if (GUI.Button(openButtonRect, string.Empty, editorSkin.GetStyle("IN ObjectField")))
            {
                SelectEntityWindow.Create(entitySelectedCallback, onDataIdentifierEntitySelectedCallback);
            }

            return value;
        }
    }
}