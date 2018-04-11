﻿
using FoxKit.Core;

using UnityEditor;

using UnityEngine;
using FoxKit.Modules.RouteBuilder.Exporter;

[CustomEditor(typeof(RouteSet))]
public class RouteSetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();

        EditorGUILayout.Space();

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
    }
}