//namespace FoxKit.Modules.PartsBuilder.FormVariation.Editor
//{
//    using UnityEngine;
//    using UnityEditor;

//    using FoxKit.Modules.PartsBuilder.FormVariation;
//    using FoxKit.Modules.PartsBuilder.FormVariation.Exporter;

//    /// <summary>
//    /// Custom editor for FormVariations.
//    /// </summary>
//    [CustomEditor(typeof(FormVariation))]
//    public class FormVariationEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            EditorGUILayout.Space();

//            if (GUILayout.Button("Export fmtt"))
//            {
//                var myTarget = (FormVariation)this.target;

//                var exportPath = EditorUtility.SaveFilePanel(
//                    "Export fv2",
//                    string.Empty,
//                    this.target.name + ".fv2",
//                    "fv2");

//                if (string.IsNullOrEmpty(exportPath))
//                {
//                    return;
//                }
//                FormVariationExporter.ExportFormVariation(myTarget as FormVariation, exportPath);
//            }

//            this.DrawDefaultInspector();
//        }
//    }
//}