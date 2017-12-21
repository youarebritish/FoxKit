using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

[CustomEditor(typeof(RouteSetImporter))]
public class RouteSetImporterEditor : ScriptedImporterEditor
{
    public override void OnInspectorGUI()
    {
        base.ApplyRevertGUI();
    }
}
