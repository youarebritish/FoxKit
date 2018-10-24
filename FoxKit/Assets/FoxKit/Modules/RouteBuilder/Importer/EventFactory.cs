using System.Linq;
using UnityEngine;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    using FoxKit.Core;

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
        /// Create a function to create RouteNodeEvents.
        /// </summary>
        /// <param name="getEventTypeName"></param>
        /// <param name="generateEventName"></param>
        /// <returns></returns>
        public static CreateEventDelegate CreateNodeEventFactory(IHashManagerExtensions.TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            return (parent, data) => CreateNodeEvent(parent, data, getEventTypeName, generateEventName);
        }

        /// <summary>
        /// Create a function to create RouteEdgeEvents.
        /// </summary>
        /// <param name="getEventTypeName"></param>
        /// <param name="generateEventName"></param>
        /// <returns></returns>
        public static CreateEventDelegate CreateEdgeEventFactory(IHashManagerExtensions.TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            return (parent, data) => CreateEdgeEvent(parent, data, getEventTypeName, generateEventName);
        }

        /// <summary>
        /// Create a RouteNodeEvent.
        /// </summary>
        /// <param name="parent">GameObject parent of the RouteEvent.</param>
        /// <param name="data">Parameters of the RouteEvent.</param>
        /// <param name="getEventTypeName">Function to get the name of the RouteEvent's type.</param>
        /// <param name="generateEventName">Function to generate a name for a RouteEvent.</param>
        /// <returns>The constructed RouteEvent.</returns>
        private static RouteNodeEvent CreateNodeEvent(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data, IHashManagerExtensions.TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            var component = parent.GetComponent<RouteNodeEvent>();
            if (component == null)
            {
                component = parent.AddComponent<RouteNodeEvent>();
            }
            var eventNameContainer = getEventTypeName(data.EventType);
            if (eventNameContainer.WasNameUnhashed)
            {
                component.Type = RouteNodeEvent.ParseEventType(eventNameContainer.UnhashedString);
            }
            else
            {
                component.Type = RouteNodeEvent.ParseEventType(eventNameContainer.Hash.ToString());
            }

            component.Name = generateEventName(RouteNodeEvent.EventTypeToString(component.Type));
            component.Params = data.Params.Cast<uint>().ToList();
            component.Snippet = data.Snippet;
            return component;
        }

        /// <summary>
        /// Create a RouteEdgeEvent.
        /// </summary>
        /// <param name="parent">GameObject parent of the RouteEvent.</param>
        /// <param name="data">Parameters of the RouteEvent.</param>
        /// <param name="getEventTypeName">Function to get the name of the RouteEvent's type.</param>
        /// <param name="generateEventName">Function to generate a name for a RouteEvent.</param>
        /// <returns>The constructed RouteEvent.</returns>
        private static RouteEdgeEvent CreateEdgeEvent(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data, IHashManagerExtensions.TryUnhashDelegate<uint> getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            var component = parent.GetComponent<RouteEdgeEvent>();
            if (component == null)
            {
                component = parent.AddComponent<RouteEdgeEvent>();
            }

            // Dumb hack to support the event with no name.
            if (data.EventType == 3205930904)
            {
                component.Type = RouteEdgeEventType.EMPTY_STRING;
            }
            // Dumb hack to support lowercase move
            else if (data.EventType == 368586264)
            {
                component.Type = RouteEdgeEventType.LOWERCASE_move;
            }
            else
            {
                var eventNameContainer = getEventTypeName(data.EventType);
                if (eventNameContainer.WasNameUnhashed)
                {
                    component.Type = RouteEdgeEvent.ParseEventType(eventNameContainer.UnhashedString);
                }
                else
                {
                    component.Type = RouteEdgeEvent.ParseEventType(eventNameContainer.Hash.ToString());
                }
            }

            component.Name = generateEventName(RouteEdgeEvent.EventTypeToString(component.Type));
            component.Params = data.Params.Cast<uint>().ToList();
            component.Snippet = data.Snippet;
            return component;
        }        
    }
}