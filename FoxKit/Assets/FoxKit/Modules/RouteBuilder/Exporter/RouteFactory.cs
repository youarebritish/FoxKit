using System.Linq;
using static FoxKit.Modules.RouteBuilder.Exporter.NodeFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    /// <summary>
    /// Collection of helper functions for constructing Routes.
    /// </summary>
    public static class RouteFactory
    {
        /// <summary>
        /// Delegate to create a Route.
        /// </summary>
        /// <param name="data">Parameters of the Route to construct.</param>
        /// <returns>The constructed Route.</returns>
        public delegate FoxLib.Tpp.RouteSet.Route CreateRouteDelegate(Route data);

        /// <summary>
        /// Delegate to get the StrCode32 hash of a Route's name.
        /// </summary>
        /// <param name="route">The Route whose name's hash to get.</param>
        /// <returns>The StrCode32 hash of the Route's name.</returns>
        public delegate uint GetRouteNameHashDelegate(Route route);

        /// <summary>
        /// Create a function to create Routes.
        /// </summary>
        /// <param name="createNode">Function to create a RouteNode.</param>
        /// <param name="getRouteNameHash">Function to get the StrCode32 hash of a Route's name.</param>
        /// <returns>Function to create a Route.</returns>
        public static CreateRouteDelegate CreateFactory(CreateNodeDelegate createNode, GetRouteNameHashDelegate getRouteNameHash)
        {
            return data => Create(data, getRouteNameHash, createNode);
        }

        /// <summary>
        /// Create a Route.
        /// </summary>
        /// <param name="data">Parameters of the Route to construct.</param>
        /// <param name="getRouteNameHash">Function to get the StrCode32 hash of a Route's name.</param>
        /// <param name="createNode">Function to create a RouteNode.</param>
        /// <returns>The constructed Route.</returns>
        private static FoxLib.Tpp.RouteSet.Route Create(Route data, GetRouteNameHashDelegate getRouteNameHash, CreateNodeDelegate createNode)
        {
            var nodes = from node in data.Nodes
                        select createNode(node);
            return new FoxLib.Tpp.RouteSet.Route(getRouteNameHash(data), nodes.ToArray());
        }
    }
}