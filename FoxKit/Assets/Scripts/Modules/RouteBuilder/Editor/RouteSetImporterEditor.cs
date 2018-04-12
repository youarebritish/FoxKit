namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Modules.RouteBuilder.Importer;
    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;

    /// <summary>
    /// Custom editor for frt importer.
    /// </summary>
    [CustomEditor(typeof(RouteSetImporter))]
    public class RouteSetImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            base.ApplyRevertGUI();
        }
    }
}