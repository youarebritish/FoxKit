namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Core;

    using UnityEditor;

    using UnityEngine;
    using FoxKit.Modules.RouteBuilder.Exporter;

    /// <summary>
    /// Custom editor for RouteNodes.
    /// </summary>
    [CustomEditor(typeof(RouteNode))]
    public class RouteNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Node"))
            {
                (this.target as RouteNode).AddNewNode();
            }
            if (GUILayout.Button("Add Event"))
            {
                (this.target as RouteNode).AddNewEvent();
            }

            EditorGUILayout.Space();

            this.DrawDefaultInspector();
        }
    }
}