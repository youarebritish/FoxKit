namespace FoxKit.Modules.RouteBuilder.Editor
{
    using FoxKit.Utils;
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Custom editor for RouteNodeEvents.
    /// </summary>
    [CustomEditor(typeof(RouteNodeEvent))]
    public class RouteNodeEventEditor : RouteEventEditor
    {
        protected override void DrawSettings()
        {
            var @event = this.target as RouteNodeEvent;
            Rotorz.Games.Collections.ReorderableListGUI.Title("Settings");

            var eventTypeContent = new GUIContent("Event type", "The type of this event.");
            @event.Type = (RouteNodeEventType)EditorGUILayout.EnumPopup(eventTypeContent, @event.Type);

            var snippetContent = new GUIContent("Snippet", "Must be a maximum of four characters.");
            @event.Snippet = EditorGUILayout.TextField(snippetContent, @event.Snippet);
        }
    }
}