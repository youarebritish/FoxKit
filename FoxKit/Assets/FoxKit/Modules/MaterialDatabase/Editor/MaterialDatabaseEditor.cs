namespace FoxKit.Modules.MaterialDatabase.Editor
{
    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.MaterialDatabase;
    using FoxKit.Modules.MaterialDatabase.Exporter;
    using FoxKit.Utils;

    /// <summary>
    /// Custom editor for MaterialDatabases.
    /// </summary>
    [CustomEditor(typeof(MaterialDatabase))]
    public class MaterialDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var asset = (MaterialDatabase)this.target;
            if (asset.IsReadOnly)
            {
                FoxKitUiUtils.ReadOnlyWarningAndButton(asset, duplicate => duplicate.IsReadOnly = false);
            }

            if (GUILayout.Button("Export fmtt"))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export fmtt",
                    string.Empty,
                    this.target.name + ".fmtt",
                    "fmtt");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }

                MaterialDatabaseExporter.ExportMaterialDatabase(asset.materialPresets, exportPath);
            }

            this.DrawDefaultInspector();
        }
    }
}