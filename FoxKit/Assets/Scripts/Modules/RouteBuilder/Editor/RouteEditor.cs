namespace FoxKit.Modules.RouteBuilder.Editor
{

    using UnityEditor;

    using UnityEngine;

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