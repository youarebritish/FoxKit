using System.Linq;
using static FoxKit.Modules.RouteBuilder.Exporter.RouteFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    public static class RouteSetFactory
    {        
        public delegate FoxLib.Tpp.RouteSet.RouteSet CreateRouteSetDelegate(RouteSet data);

        public static CreateRouteSetDelegate CreateFactory(CreateRouteDelegate createRoute)
        {
            return data => Create(data, createRoute);
        }

        private static FoxLib.Tpp.RouteSet.RouteSet Create(RouteSet data, CreateRouteDelegate createRoute)
        {
            return new FoxLib.Tpp.RouteSet.RouteSet(from route in data.Routes
                                                    select createRoute(route));
        }
    }
}