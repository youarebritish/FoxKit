namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Core;

    using UnityEditor;

    using UnityEngine;
    using FoxKit.Modules.RouteBuilder.Exporter;
    using FoxKit.Utils;

    /// <summary>
    /// Custom editor for RouteSets.
    /// </summary>
    [CustomEditor(typeof(RouteSet))]
    public class RouteSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var routeset = this.target as RouteSet;

            DrawToolShelf(routeset);
            DrawSettings(routeset);
            DrawRouteList(routeset);
        }

        private static void DrawToolShelf(RouteSet routeset)
        {
            var iconAddRoute = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_route") as Texture;
            var iconExport = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_export") as Texture;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Tools");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Add route button
            if (FoxKitUiUtils.ToolButton(iconAddRoute, "Add a new route."))
            {
                routeset.AddNewRoute();
            }

            // Export button
            if (FoxKitUiUtils.ToolButton(iconExport, "Export to .frt file."))
            {
                var exportPath = EditorUtility.SaveFilePanel(
                    "Export frt",
                    string.Empty,
                    routeset.name + ".frt",
                    "frt");

                if (string.IsNullOrEmpty(exportPath))
                {
                    return;
                }
                var hashManager = new StrCode32HashManager();
                RouteSetExporter.ExportRouteSet(routeset, hashManager, exportPath);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSettings(RouteSet routeset)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");

            var nodeEventTypeContent = new GUIContent("Default node event type", "Event type to apply to newly-created node events.");
            routeset.DefaultNodeEventType = (RouteEventType)EditorGUILayout.EnumPopup(nodeEventTypeContent, routeset.DefaultNodeEventType);

            var edgeEventTypeContent = new GUIContent("Default node event type", "Event type to apply to newly-created edge events.");
            routeset.DefaultEdgeEventType = (RouteEventType)EditorGUILayout.EnumPopup(edgeEventTypeContent, routeset.DefaultEdgeEventType);
        }

        private static void DrawRouteList(RouteSet routeset)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Routes");
            Rotorz.Games.Collections.ReorderableListGUI.ListField(routeset.Routes, CustomListItem, DrawEmpty);
        }

        private static Route CustomListItem(Rect position, Route itemValue)
        {
            return EditorGUI.ObjectField(position, itemValue, typeof(Route)) as Route;
        }

        private static void DrawEmpty()
        {
            GUILayout.Label("RouteSet has no routes.", EditorStyles.miniLabel);
        }
    }
}