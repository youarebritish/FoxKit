namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    using System;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// An event for an AI agent to perform upon arriving at a RouteNode.
    /// </summary>
    [DisallowMultipleComponent]
    public class RouteNodeEvent : RouteEvent
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public RouteNodeEventType Type = RouteNodeEventType.RelaxedIdleAct;

        /// <summary>
        /// Parse a string representation of an event type to a RouteNodeEventType.
        /// </summary>
        /// <param name="eventTypeString">String representation of a RouteNodeEventType.</param>
        /// <returns>The parsed RouteEventType.</returns>
        public static RouteNodeEventType ParseEventType(string eventTypeString)
        {
            uint hash;
            if (uint.TryParse(eventTypeString, out hash))
            {
                return (RouteNodeEventType)Enum.Parse(typeof(RouteNodeEventType), eventTypeString.Insert(0, "Unknown"));
            }
            else
            {
                return (RouteNodeEventType)Enum.Parse(typeof(RouteNodeEventType), eventTypeString);
            }
        }

        /// <summary>
        /// Converts a RouteNodeEventType to a string representation.
        /// </summary>
        /// <param name="eventType">RouteNodeEventType to convert.</param>
        /// <returns>String representation of eventType.</returns>
        public static string EventTypeToString(RouteNodeEventType eventType)
        {
            var typeString = eventType.ToString();
            if (typeString.StartsWith("Unknown"))
            {
                return typeString.Remove(0, 7);
            }
            return typeString;
        }

        /// <summary>
        /// Add a new RouteNode in the current Route.
        /// </summary>
        public override void AddNewNode()
        {
            var node = transform.parent.GetComponent<RouteNode>();
            node.AddNewNode();
        }

        /// <summary>
        /// Add a new RouteNodeEvent to the current RouteNode.
        /// </summary>
        public override void AddNewRouteNodeEvent()
        {
            var node = transform.parent.GetComponent<RouteNode>();
            node.AddNewEvent();
        }

        /// <summary>
        /// Create a new RouteNodeEvent.
        /// </summary>
        /// <param name="parent">GameObject parent to own the event.</param>
        public static RouteNodeEvent CreateNewNodeEvent(RouteNode parent, RouteNodeEventType type)
        {
            var go = new GameObject();
            go.transform.position = parent.transform.position;
            go.transform.SetParent(parent.transform);

            UnitySceneUtils.Select(go);
            SceneView.lastActiveSceneView.FrameSelected();

            var routeEvent = go.AddComponent<RouteNodeEvent>();
            routeEvent.Type = type;
            go.name = GenerateEventName(EventTypeToString(routeEvent.Type), parent.Events.Count);
            return routeEvent;
        }        
    }
}