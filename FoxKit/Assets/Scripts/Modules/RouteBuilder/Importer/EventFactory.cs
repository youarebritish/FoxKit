using System.Linq;
using UnityEngine;
using static FoxKit.Modules.RouteBuilder.Importer.RouteSetImporter;

namespace FoxKit.Modules.RouteBuilder.Importer
{
    public static class EventFactory
    {
        public delegate RouteEvent CreateEventDelegate(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent @event);

        public static CreateEventDelegate CreateFactory(TryUnhashDelegate getEventName)
        {
            return (parent, data) => Create(parent, data, getEventName);
        }

        private static RouteEvent Create(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent data, TryUnhashDelegate getEventName)
        {
            var component = parent.AddComponent<RouteEvent>();

            var eventNameContainer = getEventName(data.EventType);
            if (eventNameContainer.WasNameUnhashed)
            {
                component.Type = eventNameContainer.UnhashedString;
            }
            else
            {
                component.Type = eventNameContainer.Hash.ToString();
            }

            component.Params = data.Params.Cast<uint>().ToList();
            component.Snippet = data.Snippet;
            return component;
        }
    }
}