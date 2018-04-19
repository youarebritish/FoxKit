namespace FoxKit.Modules.RouteBuilder.Editor
{

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for RouteEvents.
    /// </summary>
    [CustomEditor(typeof(RouteEvent))]
    public class RouteEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Node"))
            {
                (this.target as RouteEvent).AddNewNode();
            }
            if (GUILayout.Button("Add Event"))
            {
                (this.target as RouteEvent).AddNewEvent();
            }

            EditorGUILayout.Space();

            this.DrawDefaultInspector();
        }
    }
}