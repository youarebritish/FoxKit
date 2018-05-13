namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.PartsBuilder.FormVariation;
    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;

    /// <summary>
    /// Custom editor for FormVariations.
    /// </summary>
    [CustomEditor(typeof(FormVariation))]
    public class FormVariationEditor : Editor
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

            serializedObject.Update();

            EditorGUILayout.Space();

            var myTarget = (FormVariation)this.target;

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

            #region HiddenMeshGroups

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            hiddenMeshGroupFoldoutStatus = EditorGUILayout.Foldout(hiddenMeshGroupFoldoutStatus, "Hidden Mesh Groups");

            {
                var hiddenMeshGroups = myTarget.HiddenMeshGroups;

                if (myTarget.HiddenMeshGroups.Count < 255)
                {
                    if (GUILayout.Button("Add"))
                    {
                        hiddenMeshGroups.Add(new HiddenMeshGroup());
                        this.Repaint();
                    }
                }

                if (myTarget.HiddenMeshGroups.Count > 0)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        hiddenMeshGroups.RemoveAt(hiddenMeshGroups.Count - 1);
                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (hiddenMeshGroupFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var hiddenMeshGroups = serializedObject.FindProperty("HiddenMeshGroups");

                    if (hiddenMeshGroups.arraySize > 0)
                    {
                        drawMeshGroups(hiddenMeshGroups, subHiddenMeshGroupFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region ShownMeshGroups

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            shownMeshGroupFoldoutStatus = EditorGUILayout.Foldout(shownMeshGroupFoldoutStatus, "Hidden Mesh Groups");

            {
                var shownMeshGroups = myTarget.ShownMeshGroups;

                if (myTarget.ShownMeshGroups.Count < 255)
                {
                    if (GUILayout.Button("Add"))
                    {
                        shownMeshGroups.Add(new ShownMeshGroup());
                        this.Repaint();
                    }
                }

                if (myTarget.ShownMeshGroups.Count > 0)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        shownMeshGroups.RemoveAt(shownMeshGroups.Count - 1);
                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (shownMeshGroupFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var shownMeshGroups = serializedObject.FindProperty("ShownMeshGroups");

                    if (shownMeshGroups.arraySize > 0)
                    {
                        drawMeshGroups(shownMeshGroups, subShownMeshGroupFoldoutStatus);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region TextureSwaps

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            textureSwapFoldoutStatus = EditorGUILayout.Foldout(textureSwapFoldoutStatus, "Texture Swaps");

            {
                var textureSwaps = myTarget.TextureSwaps;

                if (myTarget.TextureSwaps.Count < 255)
                {
                    if (GUILayout.Button("Add"))
                    {
                        textureSwaps.Add(new TextureSwap());
                        this.Repaint();
                    }
                }

                if (myTarget.TextureSwaps.Count > 0)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        textureSwaps.RemoveAt(textureSwaps.Count - 1);
                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (textureSwapFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var textureSwaps = serializedObject.FindProperty("TextureSwaps");

                    if (textureSwaps.arraySize > 0)
                    {
                        drawTextureSwaps(textureSwaps);
                    }
                }


                EditorGUI.indentLevel--;
            }

            #endregion

            #region BoneAttachments

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            boneAttachmentFoldoutStatus = EditorGUILayout.Foldout(boneAttachmentFoldoutStatus, "Bone Attachments");

            {
                var boneAttachments = myTarget.BoneAttachments;

                if (myTarget.BoneAttachments.Count < 255)
                {
                    if (GUILayout.Button("Add"))
                    {
                        boneAttachments.Add(new BoneAttachment());
                        this.Repaint();
                    }
                }

                if (myTarget.BoneAttachments.Count > 0)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        boneAttachments.RemoveAt(boneAttachments.Count - 1);
                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (boneAttachmentFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var boneAttachments = serializedObject.FindProperty("BoneAttachments");

                    if (boneAttachments.arraySize > 0)
                    {
                        drawBoneAttachments(boneAttachments);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            #region CNPAttachments

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            CNPAttachmentFoldoutStatus = EditorGUILayout.Foldout(CNPAttachmentFoldoutStatus, "CNP Attachments");

            {

                var CNPAttachments = myTarget.CNPAttachments;

                if (myTarget.CNPAttachments.Count < 255)
                {
                    if (GUILayout.Button("Add"))
                    {
                        CNPAttachments.Add(new CNPAttachment());
                        this.Repaint();
                    }
                }

                if (myTarget.CNPAttachments.Count > 0)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        CNPAttachments.RemoveAt(CNPAttachments.Count - 1);
                        this.Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            if (CNPAttachmentFoldoutStatus)
            {
                EditorGUI.indentLevel++;

                {
                    var CNPAttachments = serializedObject.FindProperty("CNPAttachments");

                    if (CNPAttachments.arraySize > 0)
                    {
                        drawCNPAttachments(CNPAttachments);
                    }
                }

                EditorGUI.indentLevel--;
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        private static void drawMeshGroups(SerializedProperty serializedProperty, bool[] meshGroupsFoldoutStatus)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                meshGroupsFoldoutStatus[i] = EditorGUILayout.Foldout(meshGroupsFoldoutStatus[i], "Element " + i);
                if (meshGroupsFoldoutStatus[i])
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("MeshGroupName"), true);
                }
            }
        }

        private static void drawTextureSwaps(SerializedProperty serializedProperty)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                subTextureSwapFoldoutStatus[i] = EditorGUILayout.Foldout(subTextureSwapFoldoutStatus[i], "Element " + i);
                if (subTextureSwapFoldoutStatus[i])
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("MaterialInstanceName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("TextureTypeName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("TextureFileName"), true);
                }
            }
        }

        private static void drawBoneAttachments(SerializedProperty serializedProperty)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                subBoneAttachmentFoldoutStatus[i] = EditorGUILayout.Foldout(subBoneAttachmentFoldoutStatus[i], "Element " + i);
                if (subBoneAttachmentFoldoutStatus[i])
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("ModelFileName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("FrdvFileName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("SimFileName"), true);
                }
            }
        }

        private static void drawCNPAttachments(SerializedProperty serializedProperty)
        {
            for (int i = 0; i < serializedProperty.arraySize; i++)
            {
                subCNPAttachmentFoldoutStatus[i] = EditorGUILayout.Foldout(subCNPAttachmentFoldoutStatus[i], "Element " + i);
                if (subCNPAttachmentFoldoutStatus[i])
                {
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("CNPName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("ModelFileName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("FrdvFileName"), true);
                    EditorGUILayout.PropertyField(serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("SimFileName"), true);
                }
            }
        }
    }
}