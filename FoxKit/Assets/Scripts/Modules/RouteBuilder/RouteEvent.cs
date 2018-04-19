namespace FoxKit.Modules.RouteBuilder
{
    using FoxKit.Utils;
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// An event for an AI agent to perform.
    /// </summary>
    [DisallowMultipleComponent]
    public class RouteEvent : MonoBehaviour
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public RouteEventType Type = RouteEventType.None;

        /// <summary>
        /// Should the event's name be treated as a hash?
        /// </summary>
        public bool TreatTypeAsHash;

        /// <summary>
        /// Deprecated. TODO: Remove this.
        /// </summary>
        [HideInInspector]
        public string Name;

        /// <summary>
        /// Event parameters. There must be exactly 10.
        /// </summary>
        public List<uint> Params = new List<uint> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Must be a maximum of four characters.
        /// </summary>
        public string Snippet = string.Empty;

        /// <summary>
        /// Parse a string representation of an event type to a RouteEventType.
        /// </summary>
        /// <param name="eventTypeString">String representation of a RouteEventType.</param>
        /// <returns>The parsed RouteEventType.</returns>
        public static RouteEventType ParseEventType(string eventTypeString)
        {
            uint hash;
            if (uint.TryParse(eventTypeString, out hash))
            {
                return (RouteEventType)System.Enum.Parse(typeof(RouteEventType), eventTypeString.Insert(0, "e"));
            }
            else
            {
                return (RouteEventType)System.Enum.Parse(typeof(RouteEventType), eventTypeString);
            }
        }

        /// <summary>
        /// Converts a RouteEventType to a string representation.
        /// </summary>
        /// <param name="eventType">RouteEventType to convert.</param>
        /// <returns>String representation of eventType.</returns>
        public static string EventTypeToString(RouteEventType eventType)
        {
            if (eventType == RouteEventType.EMPTY_STRING)
            {
                return string.Empty;
            }
            var typeString = eventType.ToString();
            if (typeString.StartsWith("e"))
            {
                return typeString.Remove(0, 1);
            }
            return typeString;
        }

        public void AddNewNode()
        {
            var node = GetComponent<RouteNode>();
            if (node == null)
            {
                node = transform.parent.GetComponent<RouteNode>();
            }
            node.AddNewNode();
        }

        public void AddNewEvent()
        {
            var node = GetComponent<RouteNode>();
            if (node == null)
            {
                node = transform.parent.GetComponent<RouteNode>();
            }
            node.AddNewEvent();
        }

        /// <summary>
        /// Create a new RouteEvent.
        /// </summary>
        /// <param name="parent">GameObject parent to own the event.</param>
        public static RouteEvent CreateNewNodeEvent(RouteNode parent, RouteEventType type)
        {
            var go = new GameObject();
            go.transform.position = parent.transform.position;
            go.transform.SetParent(parent.transform);

            UnitySceneUtils.Select(go);
            SceneView.lastActiveSceneView.FrameSelected();

            var routeEvent = go.AddComponent<RouteEvent>();
            routeEvent.Type = type;
            go.name = GenerateEventName(EventTypeToString(routeEvent.Type), parent.Events.Count);
            return routeEvent;
        }

        /// <summary>
        /// Generates a route event name.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="idGenerator">ID number generator.</param>
        /// <returns>A new route event name.</returns>
        public static string GenerateEventName(string eventType, EventIdGenerator idGenerator)
        {
            return GenerateEventName(eventType, idGenerator.Generate());
        }

        /// <summary>
        /// Generates a route event name.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">ID number of the event.</param>
        /// <returns>A new route event name.</returns>
        private static string GenerateEventName(string eventType, int id)
        {
            return String.Format("{0}_{1:0000}", eventType, id);
        }

        /// <summary>
        /// Generates route event IDs.
        /// </summary>
        public class EventIdGenerator
        {
            private int lastId;

            public int Generate()
            {
                lastId++;
                return lastId;
            }
        }
    }
}