using System.Linq;
using UnityEngine;
using static FoxKit.Core.IHashManagerExtensions;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    public static class EventFactory
    {
        public delegate RouteEvent CreateEventDelegate(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data);
        public delegate string GenerateEventNameDelegate(string eventType);

        public static CreateEventDelegate CreateFactory(TryUnhashDelegate getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            return (parent, data) => Create(parent, data, getEventTypeName, generateEventName);
        }

        private static RouteEvent Create(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data, TryUnhashDelegate getEventTypeName, GenerateEventNameDelegate generateEventName)
        {
            var component = CreateRouteSetEditor.CreateNewNodeEvent(parent);

            var eventNameContainer = getEventTypeName(data.EventType);
            if (eventNameContainer.WasNameUnhashed)
            {
                component.Type = RouteEvent.ParseEventType(eventNameContainer.UnhashedString);
            }
            else
            {
                component.Type = RouteEvent.ParseEventType(eventNameContainer.Hash.ToString());
            }

            component.Name = generateEventName(RouteEvent.EventTypeToString(component.Type));
            component.Params = data.Params.Cast<uint>().ToList();
            component.Snippet = data.Snippet;
            return component;
        }
    }
}