namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
{
    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.PartsBuilder.FormVariation;
    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;

    using Rotorz.Games.Collections;

    /// <summary>
    /// Custom editor for FormVariations.
    /// </summary>
    [CustomEditor(typeof(FormVariation))]
    public class FormVariationEditor : Editor
    {
        SerializedProperty _hiddenMeshGroupsProperty;
        SerializedProperty _shownMeshGroupsProperty;
        SerializedProperty _textureSwapProperty;
        SerializedProperty _boneAttachmentProperty;
        SerializedProperty _CNPAttachmentProperty;

        private void OnEnable()
        {
            _hiddenMeshGroupsProperty = serializedObject.FindProperty("HiddenMeshGroups");
            _shownMeshGroupsProperty = serializedObject.FindProperty("ShownMeshGroups");
            _textureSwapProperty = serializedObject.FindProperty("TextureSwaps");
            _boneAttachmentProperty = serializedObject.FindProperty("BoneAttachments");
            _CNPAttachmentProperty = serializedObject.FindProperty("CNPAttachments");
        }

        public override void OnInspectorGUI()
        {
            //Export button
            EditorGUILayout.Space();

            if (GUILayout.Button("Export fmtt"))
            {
                var myTarget = (FormVariation)this.target;

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

            //List
            drawFormVariation();
        }

        private void drawFormVariation()
        {
            serializedObject.Update();

            ReorderableListGUI.Title("Hidden Mesh Groups");
            ReorderableListGUI.ListField(_hiddenMeshGroupsProperty);

            EditorGUILayout.Space();

            ReorderableListGUI.Title("Shown Mesh Groups");
            ReorderableListGUI.ListField(_shownMeshGroupsProperty);

            EditorGUILayout.Space();

            ReorderableListGUI.Title("Texture Swaps");
            ReorderableListGUI.ListField(_textureSwapProperty);

            EditorGUILayout.Space();

            ReorderableListGUI.Title("Bone Attachments");
            ReorderableListGUI.ListField(_boneAttachmentProperty);

            EditorGUILayout.Space();

            ReorderableListGUI.Title("Connection Point Attachments");
            ReorderableListGUI.ListField(_CNPAttachmentProperty);

            serializedObject.ApplyModifiedProperties();

            base.DrawDefaultInspector();
        }
    }
}