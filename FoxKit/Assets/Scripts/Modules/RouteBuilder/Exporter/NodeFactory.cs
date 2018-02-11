using System.Linq;
using static FoxKit.Modules.RouteBuilder.Exporter.EventFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    public static class NodeFactory
    {        
        public delegate FoxLib.Tpp.RouteSet.RouteNode CreateNodeDelegate(RouteNode data);

        public static CreateNodeDelegate CreateFactory(CreateEventDelegate createEvent)
        {
            return data => Create(data, createEvent);
        }

        private static FoxLib.Tpp.RouteSet.RouteNode Create(RouteNode data, CreateEventDelegate createEvent)
        {
            return new FoxLib.Tpp.RouteSet.RouteNode(
                FoxUtils.UnityToFox(data.transform.position),
                createEvent(data.EdgeEvent), // Hold on - need to handle instancing here; this will make multiple instances
                from @event in data.Events
                select createEvent(@event)
                );
        }
    }
}