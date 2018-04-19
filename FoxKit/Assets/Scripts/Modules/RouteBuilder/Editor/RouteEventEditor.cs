namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Utils;
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
            var @event = (this.target as RouteEvent);
            DrawToolShelf(@event);
            
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");
            this.DrawDefaultInspector();
        }

        private static void DrawToolShelf(RouteEvent @event)
        {
            var iconAddNode = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_node") as Texture;
            var iconAddEvent = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_new_event") as Texture;
            var iconParent = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_parent") as Texture;
            var iconNext = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_next") as Texture;
            var iconPrev = Resources.Load("UI/Route Builder/Buttons/routebuilder_button_prev") as Texture;

            Rotorz.Games.Collections.ReorderableListGUI.Title("Tools");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // Add node button
            if (FoxKitUiUtils.ToolButton(iconAddNode, "Add a new node."))
            {
                @event.AddNewNode();
            }

            // Add event button
            if (FoxKitUiUtils.ToolButton(iconAddEvent, "Add a new event."))
            {
                @event.AddNewEvent();
            }

            // Select parent button
            if (FoxKitUiUtils.ToolButton(iconParent, "Select parent."))
            {
                UnitySceneUtils.Select(@event.transform.parent.gameObject);
            }

            // Select previous node button
            if (FoxKitUiUtils.ToolButton(iconPrev, "Select previous node."))
            {
                var node = @event.GetComponent<RouteNode>();
                if (node == null)
                {
                    node = @event.transform.parent.GetComponent<RouteNode>();
                }
                node.SelectPreviousNode();
            }

            // Select next node button
            if (FoxKitUiUtils.ToolButton(iconNext, "Select next node."))
            {
                var node = @event.GetComponent<RouteNode>();
                if (node == null)
                {
                    node = @event.transform.parent.GetComponent<RouteNode>();
                }
                node.SelectNextNode();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}