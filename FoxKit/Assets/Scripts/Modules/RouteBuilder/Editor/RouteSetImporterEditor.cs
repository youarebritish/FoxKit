using FoxKit.Modules.RouteBuilder.Importer;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

[CustomEditor(typeof(RouteSetImporter))]
public class RouteSetImporterEditor : ScriptedImporterEditor
{
    public override void OnInspectorGUI()
    {
        base.ApplyRevertGUI();
    }
}
