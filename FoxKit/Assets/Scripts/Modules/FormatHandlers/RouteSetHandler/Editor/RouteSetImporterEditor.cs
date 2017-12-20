using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

[CustomEditor(typeof(RouteSetHandler))]
public class RouteSetImporterEditor : ScriptedImporterEditor
{
    private TextAsset routeNameDictionary;
    private TextAsset eventNameDictionary;

    public override void OnInspectorGUI()
    {
        routeNameDictionary = EditorGUILayout.ObjectField("Id Dictionary", this.routeNameDictionary, typeof(TextAsset), false) as TextAsset;
        eventNameDictionary = EditorGUILayout.ObjectField("Event Dictionary", this.routeNameDictionary, typeof(TextAsset), false) as TextAsset;
        base.ApplyRevertGUI();
    }
}
