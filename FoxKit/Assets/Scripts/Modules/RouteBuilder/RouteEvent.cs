namespace FoxKit.Modules.RouteBuilder
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Base class for an event for an AI agent to perform on a Route.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class RouteEvent : MonoBehaviour
    {
        /// <summary>
        /// Should the event's name be treated as a hash?
        /// </summary>
        public bool TreatTypeAsHash;

        /// <summary>
        /// Deprecated. TODO: Remove this.
        /// </summary>
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
        /// Add a new RouteNode in the current Route.
        /// </summary>
        public abstract void AddNewNode();

        /// <summary>
        /// Add a new RouteNodeEvent to the current RouteNode.
        /// </summary>
        public abstract void AddNewRouteNodeEvent();

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
        protected static string GenerateEventName(string eventType, int id)
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