namespace FoxKit.Modules.RouteBuilder
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// An event for an AI agent to perform on the way to a RouteNode.
    /// </summary>
    [DisallowMultipleComponent]
    public class RouteEdgeEvent : RouteEvent
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public RouteEdgeEventType Type = RouteEdgeEventType.RelaxedWalk;

        public override bool TreatTypeAsHash { get { return HashedEventNames.Contains(Type); } }

        private static readonly List<RouteEdgeEventType> HashedEventNames = new List<RouteEdgeEventType>()
        {
            RouteEdgeEventType.Unknown41204288,
        };

        /// <summary>
        /// Parse a string representation of an event type to a RouteNodeEventType.
        /// </summary>
        /// <param name="eventTypeString">String representation of a RouteNodeEventType.</param>
        /// <returns>The parsed RouteEventType.</returns>
        public static RouteEdgeEventType ParseEventType(string eventTypeString)
        {
            uint hash;
            if (uint.TryParse(eventTypeString, out hash))
            {
                return (RouteEdgeEventType)Enum.Parse(typeof(RouteEdgeEventType), eventTypeString.Insert(0, "Unknown"));
            }
            else
            {
                return (RouteEdgeEventType)Enum.Parse(typeof(RouteEdgeEventType), eventTypeString);
            }
        }

        /// <summary>
        /// Converts a RouteEdgeEventType to a string representation.
        /// </summary>
        /// <param name="eventType">RouteEdgeEventType to convert.</param>
        /// <returns>String representation of eventType.</returns>
        public static string EventTypeToString(RouteEdgeEventType eventType)
        {
            if (eventType == RouteEdgeEventType.EMPTY_STRING)
            {
                return string.Empty;
            }
            if (eventType == RouteEdgeEventType.LOWERCASE_move)
            {
                return "move";
            }
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
            var node = GetComponent<RouteNode>();
            node.AddNewNode();
        }

        /// <summary>
        /// Add a new RouteNodeEvent to the current RouteNode.
        /// </summary>
        public override void AddNewRouteNodeEvent()
        {
            var node = GetComponent<RouteNode>();
            node.AddNewEvent();
        } 
    }
}