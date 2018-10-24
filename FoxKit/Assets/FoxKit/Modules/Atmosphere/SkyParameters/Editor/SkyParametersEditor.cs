namespace FoxKit.Modules.Atmosphere.SkyParameters.Editor
{
    using System.IO;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.Atmosphere.SkyParameters;
    using FoxKit.Modules.Atmosphere.SkyParameters.Exporter;
    using FoxKit.Utils;

    /// <summary>
    /// Custom editor for PrecomputedSkyParameters.
    /// </summary>
    [CustomEditor(typeof(SkyParameters))]
    public class SkyParametersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var myTarget = (SkyParameters)this.target;
            if (myTarget.IsReadOnly)
            {
                FoxKitUiUtils.ReadOnlyWarningAndButton(myTarget, asset => asset.IsReadOnly = false);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export pcsp"))
            {

                var exportPath = EditorUtility.SaveFilePanel(
                    "Export pcsp",
                    string.Empty,
                    this.target.name + ".pcsp",
                    "pcsp");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                SkyParametersExporter.ExportSkyParameters(myTarget.precomputedSkyParameters, exportPath);
            }
            else if (GUILayout.Button("Export png"))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export png",
                    string.Empty,
                    this.target.name + ".png",
                    "png");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }

                var pngPixels = (myTarget.precomputedSkyParameters).EncodeToPNG();
                File.WriteAllBytes(exportPath, pngPixels);
            }
            EditorGUILayout.EndHorizontal();

            this.DrawDefaultInspector();
        }
    }
}