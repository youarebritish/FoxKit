namespace FoxKit.Modules.RouteBuilder
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// An event for an AI agent to perform.
    /// </summary>
    public class RouteEvent : MonoBehaviour
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public RouteEventType Type = RouteEventType.None;

        /// <summary>
        /// Should the event's name be treated as a hash?
        /// </summary>
        [Tooltip("When exporting, treat the event's name as a hash instead of a string literal. Use if its true name is unknown.")]
        public bool TreatTypeAsHash;

        /// <summary>
        /// Deprecated. TODO: Remove this.
        /// </summary>
        [HideInInspector]
        public string Name;

        /// <summary>
        /// Event parameters. There must be exactly 10.
        /// </summary>
        [Tooltip("There must be exactly 10 parameters.")]
        public List<uint> Params = new List<uint> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Must be a maximum of four characters.
        /// </summary>
        [Tooltip("Must be a maximum of four characters.")]
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
    }
}