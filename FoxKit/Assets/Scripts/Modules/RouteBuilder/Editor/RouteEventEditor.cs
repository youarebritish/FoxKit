namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Utils;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for RouteEvents.
    /// </summary>
    [CustomEditor(typeof(RouteNodeEvent))]
    public class RouteNodeEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var @event = (this.target as RouteNodeEvent);

            DrawToolShelf(@event);
            DrawSettings(@event);
            DrawParams(@event);
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
            if (FoxKitUiUtils.ToolButton(iconAddEvent, "Add a new node event."))
            {
                @event.AddNewRouteNodeEvent();
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

        private static void DrawSettings(RouteNodeEvent @event)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");

            var eventTypeContent = new GUIContent("Event type", "The type of this event.");
            @event.Type = (RouteNodeEventType)EditorGUILayout.EnumPopup(eventTypeContent, @event.Type);

            var snippetContent = new GUIContent("Snippet", "Must be a maximum of four characters.");
            @event.Snippet = EditorGUILayout.TextField(snippetContent, @event.Snippet);
        }

        private static void DrawParams(RouteNodeEvent @event)
        {
            Rotorz.Games.Collections.ReorderableListGUI.Title("Parameters");

            for(int i = 0; i < 10; i++)
            {
                @event.Params[i] = (uint)EditorGUILayout.LongField($"Param {i}", @event.Params[i]);
            }
        }
    }
}