namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Utils;
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

            DrawToolShelf(route);
            DrawSettings(route);
            DrawNodeList(route);            

            EditorUtility.SetDirty(target);
        }

        private static void DrawToolShelf(Route route)
        {
            var iconAddNode = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
            var iconParent = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_parent") as Texture;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Tools");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Add node button
            if (FoxKitUiUtils.ToolButton(iconAddNode, "Add a new node."))
            {
                route.AddNewNode();
            }

            // Select parent button
            if (FoxKitUiUtils.ToolButton(iconParent, "Select parent RouteSet."))
            {
                UnitySceneUtils.Select(route.transform.parent.gameObject);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSettings(Route route)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");
            route.Closed = EditorGUILayout.Toggle("Closed", route.Closed);
            route.TreatNameAsHash = EditorGUILayout.Toggle("Treat Name as Hash", route.TreatNameAsHash);
        }

        private static void DrawNodeList(Route route)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Nodes");
            Rotorz.Games.Collections.ReorderableListGUI.ListField(route.Nodes, CustomListItem, DrawEmpty);
        }

        private static RouteNode CustomListItem(Rect position, RouteNode itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(RouteNode)) as RouteNode;
        }

        private static void DrawEmpty()
        {
            GUILayout.Label("Route has no nodes.", EditorStyles.miniLabel);
        }
    }
}