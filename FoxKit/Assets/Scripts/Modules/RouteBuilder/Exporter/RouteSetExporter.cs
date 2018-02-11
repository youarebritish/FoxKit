using System.IO;

using FoxKit.Core;

using UnityEngine.Assertions;
using System;
using System.Linq;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    public static class RouteSetExporter
    {
        public static void ExportRouteSet(RouteSet routeSet, StrCode32HashManager hashManager, string exportPath)
        {
            Assert.IsNotNull(routeSet, "RouteSet must not be null.");
            Assert.IsNotNull(hashManager, "hashManager must not be null.");
            Assert.IsNotNull(exportPath, "exportPath must not be null.");
            Assert.IsNotNull(routeSet.Routes, "RouteSet.Routes must not be null.");

            var routeCount = routeSet.Routes.Count;
            Assert.IsTrue(routeCount > 0, "Invalid route count. Cannot write a routeset with no routes.");
            Assert.IsTrue(routeCount <= ushort.MaxValue, "Invalid route count. Only up to " + ushort.MaxValue + " routes can be written to file.");

            EventFactory.GetEventTypeHashDelegate hashEventType = (@event) => GetEventTypeHash(@event, hashManager);
            RouteFactory.GetRouteNameHashDelegate hashRouteName = (route) => GetRouteNameHash(route, hashManager);

            var eventBuilder = EventFactory.CreateFactory(hashEventType);
            var nodeBuilder = NodeFactory.CreateFactory(eventBuilder);
            var routeBuilder = RouteFactory.CreateFactory(nodeBuilder, hashRouteName);
            var routeSetBuilder = RouteSetFactory.CreateFactory(routeBuilder);

            var outgoingRouteSet = routeSetBuilder(routeSet);

            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                Action<int> writeEmptyBytes = numberOfBytes => WriteEmptyBytes(writer, numberOfBytes);
                var writeFunctions = new FoxLib.Tpp.RouteSet.WriteFunctions(writer.Write, writer.Write, writer.Write, writer.Write, writer.Write, writeEmptyBytes);
                FoxLib.Tpp.RouteSet.Write(writeFunctions, outgoingRouteSet);
            }
        }

        private static uint GetRouteNameHash(Route data, StrCode32HashManager hashManager)
        {
            if (data.TreatNameAsHash)
            {
                return uint.Parse(data.name);
            }
            return hashManager.GetHash(data.name);
        }

        private static uint GetEventTypeHash(RouteEvent data, StrCode32HashManager hashManager)
        {
            if (data.TreatTypeAsHash)
            {
                return uint.Parse(data.Type);
            }
            return hashManager.GetHash(data.Type);
        }

        /// <summary>
        /// Writes a number of empty bytes.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        /// <param name="numberOfBytes">The number of empty bytes to write.</param>
        private static void WriteEmptyBytes(BinaryWriter writer, int numberOfBytes)
        {
            writer.Write(new byte[numberOfBytes]);
        }
    }
}