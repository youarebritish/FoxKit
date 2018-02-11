using System;
using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;
using static FoxKit.Modules.RouteBuilder.Importer.RouteFactory;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    public static class RouteSetFactory
    {
        public delegate RouteSet CreateRouteSetDelegate(FoxLib.Tpp.RouteSet.RouteSet data, string name);
        public delegate RouteEvent RegisterRouteEventDelegate(FoxLib.Tpp.RouteSet.RouteEvent data);

        public static CreateRouteSetDelegate CreateFactory(CreateRouteDelegate createRoute)
        {
            return (data, name) => Create(data, name, createRoute);
        }

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