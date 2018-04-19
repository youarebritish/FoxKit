using System.IO;

using FoxKit.Core;

using UnityEngine.Assertions;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.RouteBuilder.Exporter
{
    /// <summary>
    /// Collection of helper functions for exporting RouteSets to frt format.
    /// </summary>
    public static class RouteSetExporter
    {
        /// <summary>
        /// Exports a RouteSet to an frt file.
        /// </summary>
        /// <param name="routeSet">The RouteSet to export.</param>
        /// <param name="hashManager">Hash manager instance.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportRouteSet(RouteSet routeSet, StrCode32HashManager hashManager, string exportPath)
        {
            Assert.IsNotNull(routeSet, "RouteSet must not be null.");
            Assert.IsNotNull(hashManager, "hashManager must not be null.");
            Assert.IsNotNull(exportPath, "exportPath must not be null.");
            Assert.IsNotNull(routeSet.Routes, "RouteSet.Routes must not be null.");

            var routeCount = routeSet.Routes.Count;
            Assert.IsTrue(routeCount > 0, "Invalid route count. Cannot write a routeset with no routes.");
            Assert.IsTrue(routeCount <= ushort.MaxValue, "Invalid route count. Only up to " + ushort.MaxValue + " routes can be written to file.");

            EventFactory.GetNodeEventTypeHashDelegate hashNodeEventType = (@event) => GetEventTypeHash(@event, hashManager);
            EventFactory.GetEdgeEventTypeHashDelegate hashEdgeEventType = (@event) => GetEventTypeHash(@event, hashManager);
            RouteFactory.GetRouteNameHashDelegate hashRouteName = (route) => GetRouteNameHash(route, hashManager);

            var eventDictionary = new Dictionary<RouteEvent, FoxLib.Tpp.RouteSet.RouteEvent>();

            NodeFactory.TryGetEventInstanceDelegate getEventInstance = eventDictionary.TryGetValue;
            NodeFactory.RegisterEventInstanceDelegate registerEventInstance = eventDictionary.Add;            

            var eventBuilder = EventFactory.CreateFactory(hashNodeEventType, hashEdgeEventType);
            var nodeBuilder = NodeFactory.CreateFactory(getEventInstance, registerEventInstance, eventBuilder);
            var routeBuilder = RouteFactory.CreateFactory(nodeBuilder, hashRouteName);
            var routeSetBuilder = RouteSetFactory.CreateFactory(routeBuilder);

            var outgoingRouteSet = routeSetBuilder(routeSet);

            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create), FoxLib.Tpp.RouteSet.getEncoding()))
            {
                Action<int> writeEmptyBytes = numberOfBytes => WriteEmptyBytes(writer, numberOfBytes);
                var writeFunctions = new FoxLib.Tpp.RouteSet.WriteFunctions(writer.Write, writer.Write, writer.Write, writer.Write, writer.Write, writeEmptyBytes);
                FoxLib.Tpp.RouteSet.Write(writeFunctions, outgoingRouteSet);
            }
        }

        /// <summary>
        /// Get the StrCode32 hash for a route name.
        /// </summary>
        /// <param name="data">Route whose name to hash.</param>
        /// <param name="hashManager">Hash manager instance.</param>
        /// <returns>StrCode32 hash of the route name.</returns>
        private static uint GetRouteNameHash(Route data, StrCode32HashManager hashManager)
        {
            if (data.TreatNameAsHash)
            {
                return uint.Parse(data.name);
            }
            return hashManager.GetHash(data.name);
        }

        /// <summary>
        /// Get the StrCode32 hash for a node event type.
        /// </summary>
        /// <param name="data">Event whose type to hash.</param>
        /// <param name="hashManager">Hash manager instance.</param>
        /// <returns>StrCode32 hash of the event type</returns>
        private static uint GetEventTypeHash(RouteNodeEvent data, StrCode32HashManager hashManager)
        {
            if (data.TreatTypeAsHash)
            {
                return uint.Parse(RouteNodeEvent.EventTypeToString(data.Type));
            }
            return hashManager.GetHash(RouteNodeEvent.EventTypeToString(data.Type));
        }

        /// <summary>
        /// Get the StrCode32 hash for an edge event type.
        /// </summary>
        /// <param name="data">Event whose type to hash.</param>
        /// <param name="hashManager">Hash manager instance.</param>
        /// <returns>StrCode32 hash of the event type</returns>
        private static uint GetEventTypeHash(RouteEdgeEvent data, StrCode32HashManager hashManager)
        {
            if (data.TreatTypeAsHash)
            {
                return uint.Parse(RouteEdgeEvent.EventTypeToString(data.Type));
            }
            return hashManager.GetHash(RouteEdgeEvent.EventTypeToString(data.Type));
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