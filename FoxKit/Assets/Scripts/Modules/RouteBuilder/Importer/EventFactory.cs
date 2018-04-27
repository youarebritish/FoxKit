using System.Linq;
using UnityEngine;
using static FoxKit.Core.IHashManagerExtensions;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    /// <summary>
    /// Collection of helper functions for constructing RouteEvents.
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// Delegate to create a RouteEvent.
        /// </summary>
        /// <param name="parent">GameObject parent of the RouteEvent.</param>
        /// <param name="data">Parameters of the RouteEvent to create.</param>
        /// <returns>The constructed RouteEvent.</returns>
        public delegate RouteEvent CreateEventDelegate(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data);

        /// <summary>
        /// Delegate to generate a RouteEvent name.
        /// </summary>
        /// <param name="eventType">String representation of the RouteEvent type.</param>
        /// <returns>Name of the RouteEvent.</returns>
        public delegate string GenerateEventNameDelegate(string eventType);

        /// <summary>
        /// Create a function to create RouteEvents.
        /// </summary>
        /// <param name="getEventTypeName"></param>
        /// <param name="generateEventName"></param>
        /// <returns></returns>
        public static CreateEventDelegate CreateFactory(TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            return (parent, data) => Create(parent, data, getEventTypeName, generateEventName);
        }

        /// <summary>
        /// Create a RouteEvent.
        /// </summary>
        /// <param name="parent">GameObject parent of the RouteEvent.</param>
        /// <param name="data">Parameters of the RouteEvent.</param>
        /// <param name="getEventTypeName">Function to get the name of the RouteEvent's type.</param>
        /// <param name="generateEventName">Function to generate a name for a RouteEvent.</param>
        /// <returns>The constructed RouteEvent.</returns>
        private static RouteEvent Create(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data, TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            var component = parent.GetComponent<RouteEvent>();//AddComponent<RouteEvent>();
            if (component == null)
            {
                component = parent.AddComponent<RouteEvent>();
            }

            // Dumb hack to support the event with no name.
            if (data.EventType == 3205930904)
            {
                component.Type = RouteEventType.EMPTY_STRING;
            }
            else
            {
                var eventNameContainer = getEventTypeName(data.EventType);
                if (eventNameContainer.WasNameUnhashed)
                {
                    component.Type = RouteEvent.ParseEventType(eventNameContainer.UnhashedString);
                }
                else
                {
                    component.Type = RouteEvent.ParseEventType(eventNameContainer.Hash.ToString());
                }
            }

            component.Name = generateEventName(RouteEvent.EventTypeToString(component.Type));
            component.Params = data.Params.Cast<uint>().ToList();
            component.Snippet = data.Snippet;
            return component;
        }
    }
}