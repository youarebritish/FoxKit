namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Core;

    using UnityEditor;

    using UnityEngine;
    using FoxKit.Modules.RouteBuilder.Exporter;

    /// <summary>
    /// Custom editor for Routes.
    /// </summary>
    [CustomEditor(typeof(Route))]
    public class RouteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Node"))
            {
                (this.target as Route).AddNewNode();
            }

            EditorGUILayout.Space();

            this.DrawDefaultInspector();
        }
    }
}