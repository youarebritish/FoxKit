using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.EventFactory;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    public static class NodeFactory
    {
        public delegate RouteNode CreateNodeDelegate(FoxLib.Tpp.RouteSet.RouteNode data, RouteSet routeSet, Route route, string nodeName);

        public static CreateNodeDelegate CreateFactory(CreateEventDelegate createEvent)
        {
            return (data, routeSet, route, name) => Create(data, routeSet, route, name, createEvent);
        }

        private static RouteNode Create(FoxLib.Tpp.RouteSet.RouteNode data, RouteSet routeSet, Route route, string name, CreateEventDelegate createEvent)
        {
            var gameObject = new GameObject(name);
            var nodeComponent = gameObject.AddComponent<RouteNode>();
            gameObject.transform.position = FoxUtils.FoxToUnity(data.Position);
            gameObject.transform.SetParent(route.transform);

            // TODO: Handle instancing; right now this will do it once for each instance
            var edgeEvent = createEvent(routeSet.gameObject, data.EdgeEvent);

            nodeComponent.EdgeEvent = edgeEvent;
            nodeComponent.Events = (from @event in data.Events
                                    select createEvent.Invoke(gameObject, @event))
                                   .ToList();

            return nodeComponent;
        }
    }
}