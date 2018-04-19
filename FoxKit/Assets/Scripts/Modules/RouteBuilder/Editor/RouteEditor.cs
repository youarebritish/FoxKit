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
            var route = this.target as Route;

            if (GUILayout.Button("Add Node"))
            {
                route.AddNewNode();
            }

            EditorGUILayout.Space();

            route.Closed = EditorGUILayout.Toggle("Closed", route.Closed);
            route.TreatNameAsHash = EditorGUILayout.Toggle("Treat Name as Hash", route.TreatNameAsHash);

            Rotorz.Games.Collections.ReorderableListGUI.Title("Nodes");
            Rotorz.Games.Collections.ReorderableListGUI.ListField(route.Nodes, this.CustomListItem, this.DrawEmpty);

            EditorUtility.SetDirty(target);
        }

        private RouteNode CustomListItem(Rect position, RouteNode itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(RouteNode)) as RouteNode;
        }

        private void DrawEmpty()
        {
            GUILayout.Label("Route has no nodes.", EditorStyles.miniLabel);
        }
    }
}