namespace FoxKit.Modules.RouteBuilder.Exporter
{
    public static class EventFactory
    {        
        public delegate FoxLib.Tpp.RouteSet.RouteEvent CreateEventDelegate(RouteEvent data);
        public delegate uint GetEventTypeHashDelegate(RouteEvent @event);        

        public static CreateEventDelegate CreateFactory(GetEventTypeHashDelegate getEventTypeHash)
        {
            return (data) => Create(data, getEventTypeHash);
        }

        private static FoxLib.Tpp.RouteSet.RouteEvent Create(RouteEvent data, GetEventTypeHashDelegate getEventTypeHash)
        {
            return new FoxLib.Tpp.RouteSet.RouteEvent(
                getEventTypeHash(data),
                data.Params[0],
                data.Params[1],
                data.Params[2],
                data.Params[3],
                data.Params[4],
                data.Params[5],
                data.Params[6],
                data.Params[7],
                data.Params[8],
                data.Params[9],
                data.Snippet
                );
        }
    }
}