using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    /// <summary>
    /// Collection of helper functions for constructing RouteNodes.
    /// </summary>
    public static class NodeFactory
    {
        /// <summary>
        /// Delegate to create a RouteNode.
        /// </summary>
        /// <param name="data">Parameters of the RouteNode to construct.</param>
        /// <param name="routeSet">RouteSet the RouteNode belongs to.</param>
        /// <param name="route">Route the RouteNode belongs to.</param>
        /// <param name="nodeName">Name of the RouteNode.</param>
        /// <returns>The constructed RouteNode.</returns>
        public delegate RouteNode CreateNodeDelegate(FoxLib.Tpp.RouteSet.RouteNode data, RouteSet routeSet, Route route, string nodeName);

        /// <summary>
        /// Create a function to create RouteNodes.
        /// </summary>
        /// <param name="createEvent">Function to create a RouteEvent.</param>
        /// <returns>Function to create RouteNodes.</returns>
        public static CreateNodeDelegate CreateFactory(CreateEventDelegate createEvent)
        {
            return (data, routeSet, route, name) => Create(data, routeSet, route, name, createEvent);
        }

        /// <summary>
        /// Create a RouteNode.
        /// </summary>
        /// <param name="data">Parameters of the RouteNode.</param>
        /// <param name="routeSet">RouteSet the RouteNode belongs to.</param>
        /// <param name="route">Route the RouteNode belongs to.</param>
        /// <param name="name">Name of the RouteNode.</param>
        /// <param name="createEvent">Function to create a RouteEvent.</param>
        /// <returns>The constructed RouteNode.</returns>
        private static RouteNode Create(FoxLib.Tpp.RouteSet.RouteNode data, RouteSet routeSet, Route route, string name, CreateEventDelegate createEvent)
        {
            var gameObject = new GameObject(name);
            var nodeComponent = gameObject.AddComponent<RouteNode>();
            gameObject.transform.position = FoxUtils.FoxToUnity(data.Position);
            gameObject.transform.SetParent(route.transform);
            
            var edgeEvent = routeSet.RegisterRouteEvent(data.EdgeEvent, routeSet.EdgeEventsContainer.transform, createEvent);

            nodeComponent.EdgeEvent = edgeEvent;
            nodeComponent.Events = (from @event in data.Events
                                    select routeSet.RegisterRouteEvent(@event, gameObject.transform, createEvent))
                                   .ToList();

            return nodeComponent;
        }
    }
}