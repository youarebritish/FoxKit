namespace FoxKit.Modules.RouteBuilder
{
    using System.Collections.Generic;
    using UnityEngine;
    using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;

    /// <summary>
    /// A container for AI Routes.
    /// </summary>
    public class RouteSet : MonoBehaviour
    {
        /// <summary>
        /// All of the Routes contained within this RouteSet.
        /// </summary>
        public List<Route> Routes = new List<Route>();

        /// <summary>
        /// Maps registered route event data to route event instances.
        /// </summary>
        private Dictionary<FoxLib.Tpp.RouteSet.RouteEvent, RouteEvent> EdgeEvents = new Dictionary<FoxLib.Tpp.RouteSet.RouteEvent, RouteEvent>();

        /// <summary>
        /// GameObject parent to all global edge events.
        /// </summary>
        public GameObject EdgeEventsContainer;

        /// <summary>
        /// Default edge event type.
        /// </summary>
        private const RouteEventType DEFAULT_EDGE_EVENT_TYPE = RouteEventType.RelaxedWalk;

        /// <summary>
        /// Registers a route event instance.
        /// </summary>
        /// <param name="data">Raw data associated with the event.</param>
        /// <param name="parent">Parent object of the event.</param>
        /// <param name="createEvent">Function to create a new event.</param>
        /// <returns>The new, registered route event instance.</returns>
        public RouteEvent RegisterRouteEvent(FoxLib.Tpp.RouteSet.RouteEvent data, Transform parent, CreateEventDelegate createEvent)
        {
            RouteEvent eventInstance = null;
            if (EdgeEvents.TryGetValue(data, out eventInstance))
            {
                return eventInstance;
            }
            else
            {
                var eventGameObject = new GameObject();
                eventGameObject.transform.position = parent.transform.position;
                eventGameObject.transform.SetParent(parent);

                eventInstance = createEvent(eventGameObject, data);
                eventGameObject.name = eventInstance.Name;

                EdgeEvents.Add(data, eventInstance);
                return eventInstance;
            }
        }

        /// <summary>
        /// Context menu to add a new Route to the RouteSet.
        /// </summary>
        [ContextMenu("Add Route")]
        private void AddNewRoute()
        {
            var go = new GameObject();
            go.transform.SetParent(transform);
            go.name = GenerateNewRouteName(gameObject.name, Routes.Count);

            var route = go.AddComponent<Route>();
            Routes.Add(route);
        }

        /// <summary>
        /// Context menu to add a new edge event to the RouteSet.
        /// </summary>
        [ContextMenu("Add Edge Event")]
        private void AddNewEdgeEvent()
        {
            var go = new GameObject();
            go.transform.SetParent(EdgeEventsContainer.transform);
            go.name = GenerateNewEdgeEventName(EdgeEventsContainer.GetComponentsInChildren<RouteEvent>().Length);

            var edgeEvent = go.AddComponent<RouteEvent>();
            edgeEvent.Name = go.name;
            edgeEvent.Type = DEFAULT_EDGE_EVENT_TYPE;
        }

        /// <summary>
        /// Generate name for a new Route.
        /// </summary>
        /// <param name="routeSetName">Name of the RouteSet containing the new Route.</param>
        /// <param name="routeCount">Number of Routes already in the RouteSet.</param>
        /// <returns>Name for a new Route.</returns>
        private static string GenerateNewRouteName(string routeSetName, int routeCount)
        {
            return string.Format("rt_{0}_c_{1}", routeSetName, routeCount.ToString("D4"));
        }

        /// <summary>
        /// Generate name for a new edge event.
        /// </summary>
        /// <param name="eventCount">Number of edge events already in the RouteSet.</param>
        /// <returns>Name for a new edge event.</returns>
        private static string GenerateNewEdgeEventName(int eventCount)
        {
            return string.Format("{0}_{1}", DEFAULT_EDGE_EVENT_TYPE, eventCount.ToString("D4"));
        }
    }
}