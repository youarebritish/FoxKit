using System.Linq;
using static FoxKit.Modules.RouteBuilder.Exporter.NodeFactory;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    public static class RouteFactory
    {        
        public delegate FoxLib.Tpp.RouteSet.Route CreateRouteDelegate(Route data);
        public delegate uint GetRouteNameHashDelegate(Route route);

        public static CreateRouteDelegate CreateFactory(CreateNodeDelegate createNode, GetRouteNameHashDelegate getRouteIdHash)
        {
            return data => Create(data, getRouteIdHash, createNode);
        }

        private static FoxLib.Tpp.RouteSet.Route Create(Route data, GetRouteNameHashDelegate getRouteIdHash, CreateNodeDelegate createNode)
        {
            return new FoxLib.Tpp.RouteSet.Route(getRouteIdHash(data), from node in data.Nodes
                                                                          select createNode(node));
        }
    }
}