using System.Linq;
using UnityEngine;
using static FoxKit.Core.IHashManagerExtensions;
using static FoxKit.Modules.RouteBuilder.Importer.NodeFactory;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    /// <summary>
    /// Collection of helper functions for constructing Routes.
    /// </summary>
    public static class RouteFactory
    {
        /// <summary>
        /// Delegate to create a Route.
        /// </summary>
        /// <param name="data">Parameters of the Route to create.</param>
        /// <param name="routeSet">RouteSet the Route belongs to.</param>
        /// <returns>The constructed Route.</returns>
        public delegate Route CreateRouteDelegate(FoxLib.Tpp.RouteSet.Route data, RouteSet routeSet);

        /// <summary>
        /// Delegate to generate a RouteNode name.
        /// </summary>
        /// <param name="routeName">Name of the Route.</param>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>Name for the RouteNode.</returns>
        public delegate string GenerateNodeNameDelegate(string routeName, int nodeIndex);

        /// <summary>
        /// Create a function to create Routes.
        /// </summary>
        /// <param name="createNode">Function to create a RouteNode.</param>
        /// <param name="getRouteName">Function to get a Route's name.</param>
        /// <param name="generateNodeName">Function to generate a RouteNode name.</param>
        /// <returns>Function to construct Routes.</returns>
        public static CreateRouteDelegate CreateFactory(CreateNodeDelegate createNode, TryUnhashDelegate getRouteName, GenerateNodeNameDelegate generateNodeName)
        {
            return (data, routeSet) => Create(data, routeSet, createNode, getRouteName, generateNodeName);
        }

        /// <summary>
        /// Create a Route.
        /// </summary>
        /// <param name="data">Parameters of the Route to construct.</param>
        /// <param name="routeSet">RouteSet the Route belongs to.</param>
        /// <param name="createNode">Function to create a RouteNode.</param>
        /// <param name="getRouteName">Function to get a Route's name.</param>
        /// <param name="generateNodeName">Function to generate a RouteNode name.</param>
        /// <returns>The constructed Route.</returns>
        private static Route Create(FoxLib.Tpp.RouteSet.Route data, RouteSet routeSet, CreateNodeDelegate createNode, TryUnhashDelegate getRouteName, GenerateNodeNameDelegate generateNodeName)
        {
            var gameObject = new GameObject();
            var routeComponent = gameObject.AddComponent<Route>();

            var routeNameContainer = getRouteName(data.Name);
            if (routeNameContainer.WasNameUnhashed)
            {
                gameObject.name = routeNameContainer.UnhashedString;
            }
            else
            {
                gameObject.name = routeNameContainer.Hash.ToString();
                routeComponent.TreatNameAsHash = true;
            }

            routeComponent.Nodes = data.Nodes
                                    .Select((node, index) =>
                                        createNode.Invoke(node, routeSet, routeComponent, generateNodeName(gameObject.name, index)))
                                    .ToList();

            return routeComponent;
        }        
    }
}