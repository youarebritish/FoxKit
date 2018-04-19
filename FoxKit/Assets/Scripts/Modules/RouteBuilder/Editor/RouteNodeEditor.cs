namespace FoxKit.Modules.RouteBuilder.Editor
{

    using UnityEditor;

    /// <summary>
    /// Custom editor for RouteNodes.
    /// </summary>
    [CustomEditor(typeof(RouteNode))]
    public class RouteNodeEditor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            /*
            if (GUILayout.Button("Add Node"))
            {
                (this.target as RouteNode).AddNewNode();
            }
            if (GUILayout.Button("Add Event"))
            {
                (this.target as RouteNode).AddNewEvent();
            }

            EditorGUILayout.Space();*/

            this.DrawDefaultInspector();
        }
    }
}