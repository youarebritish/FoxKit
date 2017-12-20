using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

[CustomEditor(typeof(RouteSetImporter))]
public class RouteSetImporterEditor : ScriptedImporterEditor
{

    public override void OnInspectorGUI()
    {
        RouteSetImporter.RouteNameDictionary = EditorGUILayout.ObjectField("Id Dictionary", RouteSetImporter.RouteNameDictionary, typeof(TextAsset), false) as TextAsset;
        RouteSetImporter.EventNameDictionary = EditorGUILayout.ObjectField("Event Dictionary", RouteSetImporter.EventNameDictionary, typeof(TextAsset), false) as TextAsset;
        base.ApplyRevertGUI();
    }
}
