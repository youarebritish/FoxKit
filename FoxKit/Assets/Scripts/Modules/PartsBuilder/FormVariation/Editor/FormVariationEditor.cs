namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.PartsBuilder.FormVariation;
    using FoxKit.Modules.PartsBuilder.FormVariation.Importer;
    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;

    using UnityEditor.Experimental.AssetImporters;

    /// <summary>
    /// Custom editor for FormVariations.
    /// </summary>
    [CustomEditor(typeof(FormVariationImporter))]
    public class FormVariationEditor : ScriptedImporterEditor
    {
        private GUIStyle operationHeaderGUIStyle = new GUIStyle();

        //Hidden mesh groups
        private static bool hiddenMeshGroupFoldoutStatus;
        private static bool[] subHiddenMeshGroupFoldoutStatus = new bool[255];

        //Shown mesh groups
        private static bool shownMeshGroupFoldoutStatus;
        private static bool[] subShownMeshGroupFoldoutStatus = new bool[255];

        //Texture swaps
        private static bool textureSwapFoldoutStatus;
        private static bool[] subTextureSwapFoldoutStatus = new bool[255];

        //Bone attachments
        private static bool boneAttachmentFoldoutStatus;
        private static bool[] subBoneAttachmentFoldoutStatus = new bool[255];

        //CNP attachments
        private static bool CNPAttachmentFoldoutStatus;
        private static bool[] subCNPAttachmentFoldoutStatus = new bool[255];

        public override void OnInspectorGUI()
        {
            operationHeaderGUIStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.Space();

            var importer = (FormVariationImporter)this.target;

            var myTarget = AssetDatabase.LoadAssetAtPath<FormVariation>(importer.assetPath);

            if (GUILayout.Button("Export fv2"))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export fv2",
                    string.Empty,
                    this.target.name + ".fv2",
                    "fv2");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                FormVariationExporter.ExportFormVariation(myTarget as FormVariation, exportPath);
            }

            EditorGUI.BeginChangeCheck();

            #region HiddenMeshGroups

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            hiddenMeshGroupFoldoutStatus = EditorGUILayout.Foldout(hiddenMeshGroupFoldoutStatus, "Hidden Mesh Groups");

            drawTools(myTarget.HiddenMeshGroups, this);

            EditorGUILayout.EndHorizontal();

            if (hiddenMeshGroupFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var hiddenMeshGroups = serializedObject.FindProperty("HiddenMeshGroups");

                    if (hiddenMeshGroups.arraySize > 0)
                    {
                        drawProperty(hiddenMeshGroups, myTarget.HiddenMeshGroups, drawMeshGroups, subHiddenMeshGroupFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region ShownMeshGroups

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            shownMeshGroupFoldoutStatus = EditorGUILayout.Foldout(shownMeshGroupFoldoutStatus, "Shown Mesh Groups");

            drawTools(myTarget.ShownMeshGroups, this);

            EditorGUILayout.EndHorizontal();

            if (shownMeshGroupFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var shownMeshGroups = serializedObject.FindProperty("ShownMeshGroups");

                    if (shownMeshGroups.arraySize > 0)
                    {
                        drawProperty(shownMeshGroups, myTarget.ShownMeshGroups, drawMeshGroups, subShownMeshGroupFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region TextureSwaps

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            textureSwapFoldoutStatus = EditorGUILayout.Foldout(textureSwapFoldoutStatus, "Texture Swaps");

            drawTools(myTarget.TextureSwaps, this);

            EditorGUILayout.EndHorizontal();

            if (textureSwapFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var textureSwaps = serializedObject.FindProperty("TextureSwaps");

                    if (textureSwaps.arraySize > 0)
                    {
                        drawProperty(textureSwaps, myTarget.TextureSwaps, drawTextureSwaps, subTextureSwapFoldoutStatus);
                    }
                }


                EditorGUI.indentLevel--;
            }

            #endregion

            #region BoneAttachments

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            boneAttachmentFoldoutStatus = EditorGUILayout.Foldout(boneAttachmentFoldoutStatus, "Bone Attachments");

            drawTools(myTarget.BoneAttachments, this);

            EditorGUILayout.EndHorizontal();

            if (boneAttachmentFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var boneAttachments = serializedObject.FindProperty("BoneAttachments");

                    if (boneAttachments.arraySize > 0)
                    {
                        drawProperty(boneAttachments, myTarget.BoneAttachments, drawBoneAttachments, subBoneAttachmentFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region CNPAttachments

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            CNPAttachmentFoldoutStatus = EditorGUILayout.Foldout(CNPAttachmentFoldoutStatus, "CNP Attachments");

            drawTools(myTarget.CNPAttachments, this);

            EditorGUILayout.EndHorizontal();

            if (CNPAttachmentFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var CNPAttachments = serializedObject.FindProperty("CNPAttachments");

                    if (CNPAttachments.arraySize > 0)
                    {
                        drawProperty(CNPAttachments, myTarget.CNPAttachments, drawCNPAttachments, subCNPAttachmentFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Change made.");

                serializedObject.Update();
                serializedObject.ApplyModifiedProperties();

                base.Apply();

                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();
            }
        }

        private static void drawTools<TProperty>(List<TProperty> property, FormVariationEditor editor) where TProperty : new()
        {
            if (property.Count < 255)
            {
                if (GUILayout.Button("Add"))
                {
                    property.Add(new TProperty());
                    editor.Repaint();
                }
            }

            if (property.Count > 0)
            {
                if (GUILayout.Button("Remove"))
                {
                    property.RemoveAt(property.Count - 1);
                    editor.Repaint();
                }
            }
        }

        private static void drawProperty<TProperty>(SerializedProperty serializedProperty, List<TProperty> property, Action<SerializedProperty, bool, int> drawFunc, bool[] status)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                status[i] = EditorGUILayout.Foldout(status[i], "Element " + i);

                if (GUILayout.Button("Remove"))
                {
                    property.RemoveAt(i);
                }

                serializedProperty.serializedObject.ApplyModifiedProperties();

                EditorGUILayout.EndHorizontal();

                drawFunc(serializedProperty, status[i], i);
            }
        }

        private static void drawMeshGroups(SerializedProperty serializedProperty, bool status, int index)
        {
            if (status)
            {
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("MeshGroupName"), true);
            }
        }

        private static void drawTextureSwaps(SerializedProperty serializedProperty, bool status, int index)
        {
            if (status)
            {
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("MaterialInstanceName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("TextureTypeName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("TextureFileName"), true);
            }
        }

        private static void drawBoneAttachments(SerializedProperty serializedProperty, bool status, int index)
        {
            if (status)
            {
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("ModelFileName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("FrdvFileName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SimFileName"), true);
            }
        }

        private static void drawCNPAttachments(SerializedProperty serializedProperty, bool status, int index)
        {
            if (status)
            {
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("CNPName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("ModelFileName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("FrdvFileName"), true);
                EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SimFileName"), true);
            }
        }
    }
}