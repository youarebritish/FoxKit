namespace FoxKit.Modules.RouteBuilder.Editor
{
    using System.Collections.Generic;

    using FoxKit.Core;

    using UnityEditor;

    using UnityEngine;
    using FoxKit.Modules.RouteBuilder.Exporter;
    using FoxKit.Utils;
    using Rotorz.Games.Collections;

    /// <summary>
    /// Custom editor for RouteSets.
    /// </summary>
    [CustomEditor(typeof(RouteSet))]
    public class RouteSetEditor : Editor
    {
        private ReorderableListControl listControl;
        private IReorderableListAdaptor listAdaptor;

        static string filter = "";
        static List<Route> filteredRoutes;

        void OnEnable()
        {
            filteredRoutes = new List<Route>();
            var routeset = this.target as RouteSet;
            for (int c = 0; c < routeset.transform.childCount; c++)
            {
                GameObject childObject = routeset.transform.GetChild(c).gameObject;
                if (childObject.name.Contains(filter))
                {
                    childObject.SetActive(true);
                    filteredRoutes.Add(childObject.GetComponent<Route>());
                }

                //Failsafe to re-add routes to the set if it got dumped by the editor
                if (!routeset.Routes.Contains(childObject.GetComponent<Route>()))
                {
                    routeset.Routes.Add(childObject.GetComponent<Route>());
                }
            }
            
            listControl = new ReorderableListControl();
            listControl.ItemRemoving += this.OnItemRemoving;
            listAdaptor = new GenericListAdaptor<Route>(filteredRoutes, CustomListItem, ReorderableListGUI.DefaultItemHeight);
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            if (listControl != null)
            {
                listControl.ItemRemoving -= this.OnItemRemoving;
            }
        }

        private void OnItemRemoving(object sender, ItemRemovingEventArgs args)
        {
            var routeset = this.target as RouteSet;
            Route item = routeset.Routes[args.ItemIndex];
            DestroyImmediate(item.gameObject);
        }

        public override void OnInspectorGUI()
        {
            var routeset = this.target as RouteSet; //Can this be moved to OnEnable instead?

            UpdateFilter(routeset);
            DrawToolShelf(routeset);
            DrawSettings(routeset);
            DrawRouteList(routeset);
        }

        private void UpdateFilter(RouteSet routeset)
        {
            string oldFilter = filter;
            filter = EditorGUILayout.TextField("Route Filter:", filter);
            if (!oldFilter.Equals(filter)) //Only rebuild the list if the filter was changed
            {
                filteredRoutes.Clear();
                foreach (Route r in routeset.Routes)
                {
                    if (r.name.Contains(filter))
                    {
                        filteredRoutes.Add(r);
                        r.gameObject.SetActive(true);
                    }
                    else
                    {
                        r.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void DrawToolShelf(RouteSet routeset)
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

        private void DrawSettings(RouteSet routeset)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");

            var nodeEventTypeContent = new GUIContent("Default node event type", "Event type to apply to newly-created node events.");
            routeset.DefaultNodeEventType = (RouteNodeEventType)EditorGUILayout.EnumPopup(nodeEventTypeContent, routeset.DefaultNodeEventType);

            var edgeEventTypeContent = new GUIContent("Default node event type", "Event type to apply to newly-created edge events.");
            routeset.DefaultEdgeEventType = (RouteEdgeEventType)EditorGUILayout.EnumPopup(edgeEventTypeContent, routeset.DefaultEdgeEventType);
        }

        private void DrawRouteList(RouteSet routeset)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Routes"); listControl.Draw(listAdaptor);
            listControl.Draw(listAdaptor);
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