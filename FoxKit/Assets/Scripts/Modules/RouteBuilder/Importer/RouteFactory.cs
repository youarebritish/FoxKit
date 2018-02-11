using System;
using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.NodeFactory;
using static FoxKit.Modules.RouteBuilder.Importer.RouteSetImporter;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    public static class RouteFactory
    {
        public delegate Route CreateRouteDelegate(FoxLib.Tpp.RouteSet.Route data, RouteSet routeSet);
        public delegate string GenerateNodeNameDelegate(string routeName, int nodeIndex);

        public static CreateRouteDelegate CreateFactory(CreateNodeDelegate createNode, TryUnhashDelegate getRouteName, GenerateNodeNameDelegate generateNodeName)
        {
            return (data, routeSet) => Create(data, routeSet, createNode, getRouteName, generateNodeName);
        }

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