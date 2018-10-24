namespace FoxKit.Modules.Atmosphere.SkyParameters.Editor
{
    using System.IO;

    using UnityEngine;
    using UnityEditor;

    using FoxKit.Modules.Atmosphere.SkyParameters;
    using FoxKit.Modules.Atmosphere.SkyParameters.Exporter;

    /// <summary>
    /// Custom editor for PrecomputedSkyParameters.
    /// </summary>
    [CustomEditor(typeof(SkyParameters))]
    public class SkyParametersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export pcsp"))
            {
                var myTarget = (SkyParameters)this.target;

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
                var myTarget = (SkyParameters)this.target;

                var exportPath = EditorUtility.SaveFilePanel(
                    "Export png",
                    string.Empty,
                    this.target.name + ".png",
                    "png");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }

                byte[] pngPixels = (myTarget.precomputedSkyParameters).EncodeToPNG();
                File.WriteAllBytes(exportPath, pngPixels);
            }
            EditorGUILayout.EndHorizontal();

            this.DrawDefaultInspector();
        }
    }
}