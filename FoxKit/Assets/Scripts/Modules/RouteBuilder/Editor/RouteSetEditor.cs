namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Core;

    using UnityEditor;

    using UnityEngine;
    using FoxKit.Modules.RouteBuilder.Exporter;

    /// <summary>
    /// Custom editor for RouteSets.
    /// </summary>
    [CustomEditor(typeof(RouteSet))]
    public class RouteSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Route"))
            {
                (this.target as RouteSet).AddNewRoute();
            }

            if (GUILayout.Button("Export frt"))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export frt",
                    string.Empty,
                    this.target.name + ".frt",
                    "frt");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                var hashManager = new StrCode32HashManager();
                RouteSetExporter.ExportRouteSet(this.target as RouteSet, hashManager, exportPath);
            }

            EditorGUILayout.Space();

            this.DrawDefaultInspector();
        }
    }
}