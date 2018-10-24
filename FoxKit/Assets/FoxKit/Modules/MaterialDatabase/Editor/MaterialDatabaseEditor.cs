namespace FoxKit.Modules.MaterialDatabase.Editor
{
    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.MaterialDatabase;
    using FoxKit.Modules.MaterialDatabase.Exporter;

    /// <summary>
    /// Custom editor for MaterialDatabases.
    /// </summary>
    [CustomEditor(typeof(MaterialDatabase))]
    public class MaterialDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Export fmtt"))
            {
                var myTarget = (MaterialDatabase)this.target;

                var exportPath = EditorUtility.SaveFilePanel(
                    "Export fmtt",
                    string.Empty,
                    this.target.name + ".fmtt",
                    "fmtt");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                MaterialDatabaseExporter.ExportMaterialDatabase(myTarget.materialPresets as MaterialPreset[], exportPath);
            }

            this.DrawDefaultInspector();
        }
    }
}