using System.Linq;
using static FoxKit.Modules.RouteBuilder.Exporter.RouteFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    /// <summary>
    /// Collection of helper functions for constructing RouteSets.
    /// </summary>
    public static class RouteSetFactory
    { 
        /// <summary>
        /// Delegate to create a RouteSet.
        /// </summary>
        /// <param name="data">Parameters of the RouteSet to construct.</param>
        /// <returns>The constructed RouteSet.</returns>
        public delegate FoxLib.Tpp.RouteSet.RouteSet CreateRouteSetDelegate(RouteSet data);

        /// <summary>
        /// Create a function to create RouteSets.
        /// </summary>
        /// <param name="createRoute">Function to create a Route.</param>
        /// <returns>Function to create a RouteSet.</returns>
        public static CreateRouteSetDelegate CreateFactory(CreateRouteDelegate createRoute)
        {
            return data => Create(data, createRoute);
        }

        /// <summary>
        /// Create a RouteSet.
        /// </summary>
        /// <param name="data">Parameters of the RouteSet to construct.</param>
        /// <param name="createRoute">Function to create a Route.</param>
        /// <returns>The constructed RouteSet.</returns>
        private static FoxLib.Tpp.RouteSet.RouteSet Create(RouteSet data, CreateRouteDelegate createRoute)
        {
            var routes = from route in data.Routes
                         select createRoute(route);
            var routeset = new FoxLib.Tpp.RouteSet.RouteSet(routes.ToArray());

            return routeset;
        }
    }
}