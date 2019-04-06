namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Core;
    using FoxKit.Modules.PartsBuilder.FormVariation;
    using FoxKit.Modules.PartsBuilder.FormVariation.Importer;
    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;
    using FoxKit.Utils;

    using UnityEditor.Experimental.AssetImporters;

    using Rotorz.Games.Collections;
    using FoxKit.Core.WIP;

    /// <summary>
    /// Custom editor for FormVariations.
    /// </summary>
    [CustomEditor(typeof(FormVariation))]
    public class FormVariationEditor : Editor
    {
        #region Properties
        private FormVariation myTarget;
        private FormVariationCategory currentCategory = FormVariationCategory.STATIC;
        private FormVariationOptionSet currentOptionSet;
        private FormVariationOptionSetOption currentOption;
        private int optionIndex = 0;
        private int toolbarStatus = 0;
        private static GUIStyle categoryIndexFieldStyle;
        private static GUIStyle categoryMaxLabelStyle;
        #endregion

        public void OnEnable()
        {
            myTarget = this.target as FormVariation;
            currentOptionSet = myTarget.Options[currentCategory];
            currentOption = currentOptionSet.Options[optionIndex];

            categoryIndexFieldStyle = EditorStyles.textField;
            categoryIndexFieldStyle.alignment = TextAnchor.MiddleRight;
            categoryMaxLabelStyle = EditorStyles.label;
            categoryMaxLabelStyle.alignment = TextAnchor.MiddleLeft;
        }

        public override void OnInspectorGUI()
        {
            DrawExportButton();

            EditorGUILayout.Space();

            DrawCategory();

            EditorGUILayout.Space();

            if (currentCategory != FormVariationCategory.STATIC)
                DrawOptionSelector(EditorGUILayout.GetControlRect(false, 40));

            DrawToolbar();

            // Begin change check
            EditorGUI.BeginChangeCheck();

            switch (toolbarStatus)
            {
                case 0:
                    //EditorGUILayout.BeginHorizontal();
                    DrawProperties("Hide", currentOption.HiddenMeshGroups, DrawMeshGroup);
                    DrawProperties("Show", currentOption.ShownMeshGroups, DrawMeshGroup);
                    //EditorGUILayout.EndHorizontal();
                    break;
                case 1:
                    DrawProperties("Texture Swaps", currentOption.TextureSwaps, DrawTextureSwap);
                    break;
                default:
                    DrawProperties("Bone Attachments", currentOption.BoneAttachments, DrawBoneAttachment);
                    DrawProperties("CNP Attachments", currentOption.CNPAttachments, DrawCNPAttachment);
                    break;
            }

            // End change check
            ChangeCheck();
        }

        #region Regions
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

        private void DrawCategory()
        {
            UpdateCurrentCategory((FormVariationCategory)EditorGUILayout.EnumPopup(currentCategory));
        }

        private void DrawToolbar()
        {
            EditorGUILayout.Space();

            var toolbarTitles = new string[] { $"Visibility ({currentOption.HiddenMeshGroups.Count + currentOption.ShownMeshGroups.Count})", $"Texture Swaps ({currentOption.TextureSwaps.Count})", $"Attachments ({currentOption.BoneAttachments.Count + currentOption.CNPAttachments.Count})" };
            toolbarStatus = GUILayout.Toolbar(toolbarStatus, toolbarTitles);
        }

        private void DrawOptionSelector(Rect rect)
        {
            Rect previousRect = rect;
            previousRect.width = previousRect.height;
            if (GUI.Button(previousRect, "<<") && optionIndex > 0)
            {
                UpdateCurrentOption(optionIndex - 1);
            }

            Rect optionIndexRect = previousRect;
            optionIndexRect.x += optionIndexRect.width + 4.0f;
            optionIndex = EditorGUI.IntField(optionIndexRect, optionIndex + 1, categoryIndexFieldStyle) - 1;

            Rect optionIndexMaxRect = optionIndexRect;
            optionIndexMaxRect.x += optionIndexMaxRect.width;
            EditorGUI.LabelField(optionIndexMaxRect, $"of {currentOptionSet.Options.Count}", categoryMaxLabelStyle);

            Rect nextRect = optionIndexMaxRect;
            nextRect.x += nextRect.width + 4.0f;
            if (GUI.Button(nextRect, ">>") && optionIndex != currentOptionSet.Options.Count - 1)
            {
                UpdateCurrentOption(optionIndex + 1);
            }

            Rect removeRect = nextRect;
            removeRect.x += removeRect.width + 4.0f;
            if (GUI.Button(removeRect, "-") && currentOptionSet.Options.Count > 1)
            {
                currentOptionSet.Options.RemoveAt(optionIndex);
                UpdateCurrentOption(optionIndex - 1);
            }

            Rect addRect = removeRect;
            addRect.x += addRect.width + 4.0f;
            if (GUI.Button(addRect, "+"))
            {
                currentOptionSet.Options.Add(new FormVariationOptionSetOption());
                UpdateCurrentOptionLast();
            }
        }

        private void DrawProperties<T>(string title, List<T> properties, Func<Rect, T, T> draw) where T : new()
        {
            ReorderableListGUI.Title(title);

            ReorderableListGUI.ListField
            (
                properties,
                (rect, property) => draw(rect, property),
                () => EditorGUILayout.LabelField("Nothing")
            );
        }
        #endregion

        #region Helpers
        private void UpdateCurrentCategory(FormVariationCategory category)
        {
            currentCategory = category;
            currentOptionSet = myTarget.Options[currentCategory];
            if (optionIndex > currentOptionSet.Options.Count - 1)
                UpdateCurrentOptionLast();
        }

        private void UpdateCurrentOptionLast()
        {
            optionIndex = currentOptionSet.Options.Count - 1;
            currentOption = currentOptionSet.Options[optionIndex];
        }

        private void UpdateCurrentOption(int index)
        {
            optionIndex = index;
            currentOption = currentOptionSet.Options[optionIndex];
        }

        private void ChangeCheck()
        {
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(myTarget);
            }
        }
        #endregion

        #region Visibility
        private MeshGroup DrawMeshGroup(Rect rect, MeshGroup meshGroup)
        {
            // #UPDATEANDREPLACE#
            if (meshGroup == null)
                meshGroup = new MeshGroup();

            // Name field
            Rect middlePropertyRect = rect;
            middlePropertyRect.width = middlePropertyRect.width;
            meshGroup.MeshGroupName = EditorGUI.TextField(middlePropertyRect, (meshGroup.MeshGroupName == null ? "" : (string)meshGroup.MeshGroupName));

            return meshGroup;
        }
        #endregion

        #region TextureSwaps
        private TextureSwap DrawTextureSwap(Rect rect, TextureSwap textureSwap)
        {
            // #UPDATEANDREPLACE#
            if (textureSwap == null)
                textureSwap = new TextureSwap();

            var padding = rect.width * (0.05f / 6.0f);
            var width = 2f / 6.1f;

            // Material popup
            Rect firstPropertyRect = rect;
            float offset = 0;
            firstPropertyRect.y += firstPropertyRect.height * 0.09f;
            firstPropertyRect.width = firstPropertyRect.width * (2.0f / 6.0f);
            
            textureSwap.MaterialInstanceName = (string)FormVariationEditorUtils.ObjectStringField<Material>((string)textureSwap.MaterialInstanceName, firstPropertyRect, ParseTextureList);
            //textureSwap.MaterialInstance = (Material)EditorGUI.ObjectField(firstPropertyRect, textureSwap.MaterialInstance, typeof(Material), false);

            offset += firstPropertyRect.width + padding;

            // Texture type popup
            Rect middlePropertyRect = rect;
            middlePropertyRect.x += offset;
            middlePropertyRect.y += middlePropertyRect.height * 0.145f;
            middlePropertyRect.width = middlePropertyRect.width * (2.3f / 6.0f);
            if (textureSwap.MaterialInstance == null)
            {
                GUI.enabled = false;
                EditorGUI.Popup(middlePropertyRect, 0, new string[] { "" });
                GUI.enabled = true;
            }
            else
            {
                var textureList = ParseTextureList(textureSwap.MaterialInstance);
                var index = textureSwap.TextureType == null ? 0 : Array.IndexOf(textureList, (string)textureSwap.TextureType);
                textureSwap.TextureType = textureList[EditorGUI.Popup(middlePropertyRect, index, textureList)];
            }

            offset += middlePropertyRect.width + padding;

            // Texture path field
            Rect lastPropertyRect = rect;
            lastPropertyRect.x += offset;
            lastPropertyRect.y += lastPropertyRect.height * 0.09f;
            lastPropertyRect.width = lastPropertyRect.width * (1.6f / 6.0f);
            lastPropertyRect.height = 15;
            textureSwap.Texture = (Texture)EditorGUI.ObjectField(lastPropertyRect, textureSwap.Texture, typeof(Texture), false);

            return textureSwap;
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
        #endregion

        #region Attachments
        private BoneAttachment DrawBoneAttachment(Rect rect, BoneAttachment boneAttachment)
        {
            // #UPDATEANDREPLACE#
            if (boneAttachment == null)
                boneAttachment = new BoneAttachment();

            var padding = rect.width * (0.05f / 6.0f);
            var width = 2f / 6.1f;

            // Model file name
            Rect firstPropertyRect = rect;
            float offset = 0;
            firstPropertyRect.width = firstPropertyRect.width * width;
            boneAttachment.ModelFileName = EditorGUI.TextField(firstPropertyRect, (boneAttachment.ModelFileName == null ? "" : (string)boneAttachment.ModelFileName));

            offset += firstPropertyRect.width + padding;

            // Sim file name
            Rect middlePropertyRect = rect;
            middlePropertyRect.x += offset;
            middlePropertyRect.width = middlePropertyRect.width * width;
            {
                var result = EditorGUI.TextField(middlePropertyRect, (boneAttachment.FrdvFileName == null ? "" : (string)boneAttachment.FrdvFileName));
                boneAttachment.FrdvFileName = result == "" ? null : (Core.WIP.PathFileNameCode64HashPair)result;
            }

            offset += middlePropertyRect.width + padding;

            // Frdv file name
            Rect lastPropertyRect = rect;
            lastPropertyRect.x += offset;
            lastPropertyRect.width = lastPropertyRect.width * width;
            {
                var result = EditorGUI.TextField(lastPropertyRect, (boneAttachment.SimFileName == null ? "" : (string)boneAttachment.SimFileName));
                boneAttachment.SimFileName = result == "" ? null : (Core.WIP.PathFileNameCode64HashPair)result;
            }

            return boneAttachment;
        }

        private CNPAttachment DrawCNPAttachment(Rect rect, CNPAttachment CNPAttachment)
        {
            // #UPDATEANDREPLACE#
            if (CNPAttachment == null)
                CNPAttachment = new CNPAttachment();

            var padding = rect.width * (0.05f / 6.0f);
            var width = 2f / (7.15f);

            // CNP file name
            Rect firstPropertyRect = rect;
            float offset = 0;
            firstPropertyRect.width = firstPropertyRect.width * (0.8f / 6.0f);
            CNPAttachment.CNPName = EditorGUI.TextField(firstPropertyRect, (CNPAttachment.CNPName == null ? "" : (string)CNPAttachment.CNPName));

            offset += firstPropertyRect.width + padding;

            // Model file name
            Rect secondPropertyRect = rect;
            secondPropertyRect.x += offset;
            secondPropertyRect.width = secondPropertyRect.width * width;
            CNPAttachment.ModelFileName = EditorGUI.TextField(secondPropertyRect, (CNPAttachment.ModelFileName == null ? "" : (string)CNPAttachment.ModelFileName));

            offset += secondPropertyRect.width + padding;

            // Sim file name
            Rect middlePropertyRect = rect;
            middlePropertyRect.x += offset;
            middlePropertyRect.width = middlePropertyRect.width * width;
            {
                var result = EditorGUI.TextField(middlePropertyRect, (CNPAttachment.FrdvFileName == null ? "" : (string)CNPAttachment.FrdvFileName));
                CNPAttachment.FrdvFileName = result == "" ? null : (Core.WIP.PathFileNameCode64HashPair)result;
            }

            offset += middlePropertyRect.width + padding;

            // Frdv file name
            Rect lastPropertyRect = rect;
            lastPropertyRect.x += offset;
            lastPropertyRect.width = lastPropertyRect.width * width;
            {
                var result = EditorGUI.TextField(lastPropertyRect, (CNPAttachment.SimFileName == null ? "" : (string)CNPAttachment.SimFileName));
                CNPAttachment.SimFileName = result == "" ? null : (Core.WIP.PathFileNameCode64HashPair)result;
            }

            return CNPAttachment;
        }
        #endregion
    }
}