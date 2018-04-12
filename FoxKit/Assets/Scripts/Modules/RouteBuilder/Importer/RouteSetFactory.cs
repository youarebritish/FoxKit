using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.RouteFactory;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    /// <summary>
    /// Collection of helper functions for constructing RouteSets.
    /// </summary>
    public static class RouteSetFactory
    {
        /// <summary>
        /// Delegate to create a RouteSet.
        /// </summary>
        /// <param name="data">Parameters of the RouteSet to create.</param>
        /// <param name="name">Name of the RouteSet to create.</param>
        /// <returns>The created RouteSet.</returns>
        public delegate RouteSet CreateRouteSetDelegate(FoxLib.Tpp.RouteSet.RouteSet data, string name);

        /// <summary>
        /// Delegate to register a RouteEvent instance.
        /// </summary>
        /// <param name="data">RouteEvent to register.</param>
        /// <returns>Instance of the registered RouteEvent.</returns>
        public delegate RouteEvent RegisterRouteEventDelegate(FoxLib.Tpp.RouteSet.RouteEvent data);

        /// <summary>
        /// Create a function to create RouteSets.
        /// </summary>
        /// <param name="createRoute">Function to create a Route.</param>
        /// <returns>Function to construct RouteSets.</returns>
        public static CreateRouteSetDelegate CreateFactory(CreateRouteDelegate createRoute)
        {
            return (data, name) => Create(data, name, createRoute);
        }

        /// <summary>
        /// Create a RouteSet.
        /// </summary>
        /// <param name="data">Parameters of the RouteSet to create.</param>
        /// <param name="name">Name of the RouteSet to create.</param>
        /// <param name="createRoute">Function to create a Route.</param>
        /// <returns>The constructed RouteSet.</returns>
        private static RouteSet Create(FoxLib.Tpp.RouteSet.RouteSet data, string name, CreateRouteDelegate createRoute)
        {
            var gameObject = new GameObject(name);

            var routeSetComponent = gameObject.AddComponent<RouteSet>();
            routeSetComponent.EdgeEventsContainer = new GameObject("Edge Events");
            routeSetComponent.EdgeEventsContainer.transform.SetParent(gameObject.transform);

            routeSetComponent.Routes = (from route in data.Routes
                                        select createRoute(route, routeSetComponent))
                                       .ToList();

            foreach (var routeGameObject in routeSetComponent.Routes)
            {
                routeGameObject.transform.SetParent(gameObject.transform);
            }

            return routeSetComponent;
        }
    }
}