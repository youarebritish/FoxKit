namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.PartsBuilder.FormVariation;
    using FoxKit.Modules.PartsBuilder.FormVariation.Importer;
    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;
    using FoxKit.Utils;

    using UnityEditor.Experimental.AssetImporters;

    /// <summary>
    /// Custom editor for FormVariations.
    /// </summary>
    [CustomEditor(typeof(FormVariation))]
    public class FormVariationEditor : Editor
    {
        private static FormVariation myTarget;

        private readonly string[] toolbarTitles = new string[] { "Mesh Groups", "Texture Swaps", "Attachments" };
        private int toolbarStatus = 0;

        public void OnEnable()
        {
            myTarget = this.target as FormVariation;
        }

        public override void OnInspectorGUI()
        {
            Header();

            DrawExportButton();

            DrawToolbar();

            // Begin change check
            EditorGUI.BeginChangeCheck();

            switch (toolbarStatus)
            {
                case 0:
                    break;
                case 1:
                    DrawTextureSwaps();
                    break;
                default:
                    break;
            }

            // End change check
            ChangeCheck();
        }

        private void Header()
        {
            EditorGUILayout.Space();
        }

        private void DrawExportButton()
        {
            if (GUILayout.Button("Export fv2"))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export fv2",
                    string.Empty,
                    base.target.name + ".fv2",
                    "fv2");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                FormVariationExporter.ExportFormVariation(myTarget, exportPath);
            }
        }

        private void DrawToolbar()
        {
            toolbarStatus = GUILayout.Toolbar(toolbarStatus, toolbarTitles);
        }

        private void DrawTextureSwaps()
        {
            var properties = serializedObject.FindProperty("TextureSwaps");
            if (properties.arraySize > 0)
            {
                for (var i = 0; i < properties.arraySize; i++)
                {
                    DrawTextureSwap(properties.GetArrayElementAtIndex(i), myTarget.TextureSwaps[i].MaterialInstance);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Array empty!");

                if (GUILayout.Button("  +  "))
                {
                    (myTarget).TextureSwaps.Add(new TextureSwap());
                }
            }
        }

        private void DrawTextureSwap(SerializedProperty textureSwapProperty, Material material)
        {
            var materialProperty = textureSwapProperty.FindPropertyRelative("MaterialInstance");
            EditorGUILayout.PropertyField(materialProperty);
            if (material == null)
            {
                EditorGUILayout.Popup(0, new string[] { "" });
            }
            else
            {
                GUI.enabled = false;
                EditorGUILayout.Popup(0, ParseTextureList(material));
                GUI.enabled = true;
            }
        }

        private static string[] ParseTextureList(Material material)
        {
            var shader = material.shader;

            var propertyCount = ShaderUtil.GetPropertyCount(shader);
            List<string> properties = new List<string>();
            for (int i = 0; i < propertyCount; i++)
            {
                var property = ShaderUtil.GetPropertyType(shader, i);
                if (property == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    properties.Add(ShaderUtil.GetPropertyName(shader, i));
                }
            }

            return properties.ToArray();
        }

        private void ChangeCheck()
        {
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Changes detected!");

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                //base.Apply();

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }
        }
    }
}