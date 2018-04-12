using System.Collections.Generic;
using static FoxKit.Modules.RouteBuilder.Exporter.EventFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    /// <summary>
    /// Collection of helper functions for constructing RouteNodes.
    /// </summary>
    public static class NodeFactory
    {        
        public delegate FoxLib.Tpp.RouteSet.RouteNode CreateNodeDelegate(RouteNode data);

        /// <summary>
        /// Tries to get a preexisting instance of a given event.
        /// </summary>
        /// <param name="data">The event data to get an instance of.</param>
        /// <param name="instance">The preexisting instance, or null if none has been created.</param>
        /// <returns>True if an instance was found, else false.</returns>
        public delegate bool TryGetEventInstanceDelegate(RouteEvent data, out FoxLib.Tpp.RouteSet.RouteEvent instance);

        /// <summary>
        /// Registers an event instance so that it can be later retrieved by nodes sharing events.
        /// </summary>
        /// <param name="data">The event data to register.</param>
        /// <param name="instance">The instance to register.</param>
        public delegate void RegisterEventInstanceDelegate(RouteEvent data, FoxLib.Tpp.RouteSet.RouteEvent instance);

        /// <summary>
        /// Create a function to create RouteNode.
        /// </summary>
        /// <param name="getEventInstance">Function to get an existing RouteEvent instance.</param>
        /// <param name="registerEventInstance">Function to register a RouteEvent instance.</param>
        /// <param name="createEvent">Function to create a RouteEvent.</param>
        /// <returns>Function to create a RouteNode</returns>
        public static CreateNodeDelegate CreateFactory(TryGetEventInstanceDelegate getEventInstance, RegisterEventInstanceDelegate registerEventInstance, CreateEventDelegate createEvent)
        {
            return data => Create(data, getEventInstance, registerEventInstance, createEvent);
        }

        /// <summary>
        /// Create a RouteNode.
        /// </summary>
        /// <param name="data">Parameters of the RouteNode to construct.</param>
        /// <param name="getEventInstance">Function to get an existing RouteEvent instance.</param>
        /// <param name="registerEventInstance">Function to register a RouteEvent instance.</param>
        /// <param name="createEvent">Function to create a RouteEvent.</param>
        /// <returns>Function to create a RouteNode.</returns>
        private static FoxLib.Tpp.RouteSet.RouteNode Create(RouteNode data, TryGetEventInstanceDelegate getEventInstance, RegisterEventInstanceDelegate registerEventInstance, CreateEventDelegate createEvent)
        {
            FoxLib.Tpp.RouteSet.RouteEvent edgeEvent;
            if (!getEventInstance(data.EdgeEvent, out edgeEvent))
            {
                edgeEvent = createEvent(data.EdgeEvent);
                registerEventInstance(data.EdgeEvent, edgeEvent);
            }

            List<FoxLib.Tpp.RouteSet.RouteEvent> eventInstances = new List<FoxLib.Tpp.RouteSet.RouteEvent>();
            foreach (var @event in data.Events)
            {
                FoxLib.Tpp.RouteSet.RouteEvent eventInstance;
                if (!getEventInstance(@event, out eventInstance))
                {
                    var newInstance = createEvent(@event);
                    registerEventInstance(@event, newInstance);
                    eventInstances.Add(newInstance);
                    continue;
                }
                eventInstances.Add(eventInstance);
            }

            return new FoxLib.Tpp.RouteSet.RouteNode(
                FoxUtils.UnityToFox(data.transform.position),
                edgeEvent,
                eventInstances.ToArray()
                );
        }
    }
}