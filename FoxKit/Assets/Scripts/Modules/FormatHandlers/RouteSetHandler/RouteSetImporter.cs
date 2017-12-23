namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using FoxKit.Core;

    using GzsTool.Core;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> eventNameHashManager;

        private IHashManager<uint> messageHashManager;

        private IHashManager<uint> routeNameHashManager;

        private delegate float ReadFloat();

        private delegate RouteDefinition ReadRouteDefinition();

        private delegate void SetPosition(Vector3 newPosition);

        private delegate void SetTransformParent(Transform childTransform);

        private delegate uint GetRouteNodeCount(Route route);

        public override void OnImportAsset(AssetImportContext ctx)
        {
            this.InitializeDictionaries();

            var idHashDump = RouteSetImporterPreferences.Instance.IdHashDump.GetUniqueLines();
            var eventNameHashDump = RouteSetImporterPreferences.Instance.EventHashDump.GetUniqueLines();
            var eventSnippetDump = RouteSetImporterPreferences.Instance.JsonDump.GetUniqueLines();

            var path = ctx.assetPath;
            var routesetName = Path.GetFileNameWithoutExtension(path);
            var routeset = ReadRouteSet(
                ctx,
                routesetName,
                this.routeNameHashManager,
                this.eventNameHashManager,
                idHashDump,
                eventNameHashDump,
                eventSnippetDump);

            RouteSetImporterPreferences.Instance.IdHashDump.Overwrite(idHashDump);
            RouteSetImporterPreferences.Instance.EventHashDump.Overwrite(eventNameHashDump);
            RouteSetImporterPreferences.Instance.JsonDump.Overwrite(eventSnippetDump);

            ctx.AddObjectToAsset(routeset.name, routeset.gameObject);
            ctx.SetMainObject(routeset.gameObject);
        }

        private static RouteSet ReadRouteSet(
            AssetImportContext ctx,
            string routesetName,
            IHashManager<uint> routeNameHashManager,
            IHashManager<uint> eventNameHashManager,
            ISet<string> idHashDump,
            ISet<string> eventNameHashDump,
            ISet<string> eventSnippetDump)
        {
            var routeset = CreateRouteSet(routesetName);
            routeset.transform.position = Vector3.zero;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                routeset.Routes = ReadHeader(reader, routeNameHashManager, idHashDump);
                foreach (var route in routeset.Routes)
                {
                    route.transform.SetParent(routeset.transform);
                }

                var nodeCount = new Dictionary<Route, ushort>();
                var eventCount = new Dictionary<Route, ushort>();

                ReadRouteDefinitions(routeset, reader, nodeCount, eventCount);
                ReadNodes(routeset, reader.ReadSingle, route => nodeCount[route]);

                var eventTable = ReadEventTable(routeset, reader);
                var routeEvents = ReadEvents(
                    eventCount,
                    reader,
                    eventNameHashManager,
                    eventNameHashDump,
                    eventSnippetDump);
                AssignEvents(routeset, eventTable, routeEvents);
            }

            return routeset;
        }

        private static List<Route> ReadHeader(
            BinaryReader reader,
            IHashManager<uint> routeNameHashManager,
            ISet<string> idHashDump)
        {
            reader.Skip(4);

            var version = reader.ReadInt16();
            if (version != 3)
            {
                throw new InvalidDataException("Frt version " + version + " is not supported.");
            }

            var routeIdCount = reader.ReadInt16();
            var routes = new List<Route>(routeIdCount);

            reader.Skip(5 * sizeof(int));

            for (var i = 0; i < routeIdCount; i++)
            {
                var route = ReadRoute(reader, routeNameHashManager, idHashDump);
                routes.Add(route);
            }

            return routes;
        }

        private static Route ReadRoute(
            BinaryReader reader,
            IHashManager<uint> routeNameHashManager,
            ISet<string> idHashDump)
        {
            var routeNameHash = reader.ReadUInt32();
            string routeNameString;
            if (!routeNameHashManager.TryGetStringFromHash(routeNameHash, out routeNameString))
            {
                routeNameString = routeNameHash.ToString();

                if (!idHashDump.Contains(routeNameString))
                {
                    idHashDump.Add(routeNameString);
                }
            }

            var route = CreateRoute(routeNameString);
            return route;
        }

        private static void ReadRouteDefinitions(
            RouteSet routeset,
            BinaryReader reader,
            IDictionary<Route, ushort> nodeCount,
            IDictionary<Route, ushort> eventCount)
        {
            foreach (var route in routeset.Routes)
            {
                var routeMetadata = ReadRouteMetadata(() => RouteDefinition.Read(reader));
                nodeCount.Add(route, routeMetadata.Item1);
                eventCount.Add(route, routeMetadata.Item2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Number of nodes in the route and number of events in the route.</returns>
        private static Tuple<ushort, ushort> ReadRouteMetadata(ReadRouteDefinition readRouteDefinition)
        {
            var routeDefinition = readRouteDefinition();
            return Tuple.Create(routeDefinition.NumEdgeEvents, routeDefinition.NumEvents);
        }

        private static void ReadNodes(RouteSet routeset, ReadFloat readFloat, GetRouteNodeCount getRouteNodeCount)
        {
            foreach (var route in routeset.Routes)
            {
                route.Nodes = ReadNodesForRoute(
                    readFloat,
                    position => route.transform.position = position,
                    nodeTransform => nodeTransform.SetParent(route.transform),
                    getRouteNodeCount(route),
                    route.name);
            }
        }

        private static Dictionary<RouteNode, RouteNodeEventData> ReadEventTable(RouteSet routeset, BinaryReader reader)
        {
            var nodeEventDataTable = new Dictionary<RouteNode, RouteNodeEventData>();
            foreach (var route in routeset.Routes)
            {
                foreach (var node in route.Nodes)
                {
                    var nodeEventData = RouteNodeEventData.Read(reader);
                    nodeEventDataTable.Add(node, nodeEventData);
                }
            }

            return nodeEventDataTable;
        }

        private static List<RouteEvent> ReadEvents(
            Dictionary<Route, ushort> eventCount,
            BinaryReader reader,
            IHashManager<uint> eventNameHashManager,
            ISet<string> eventNameHashDump,
            ISet<string> eventSnippetDump)
        {
            var routeEvents = new List<RouteEvent>();
            foreach (var route in eventCount.Keys)
            {
                for (var i = 0; i < eventCount[route]; i++)
                {
                    var eventNameHash = reader.ReadUInt32();
                    string eventNameString;
                    if (!eventNameHashManager.TryGetStringFromHash(eventNameHash, out eventNameString))
                    {
                        eventNameString = eventNameHash.ToString();
                        if (!eventNameHashDump.Contains(eventNameString))
                        {
                            eventNameHashDump.Add(eventNameString);
                        }
                    }

                    var routeEvent = new RouteEvent { Name = eventNameString };

                    for (var j = 0; j < 10; j++)
                    {
                        routeEvent.Params.Add(reader.ReadUInt32());
                    }

                    var snippet = reader.ReadBytes(4);
                    routeEvent.Snippet = Encoding.Default.GetString(snippet);
                    if (routeEvent.Snippet[0] == '\0' && !eventSnippetDump.Contains(routeEvent.Snippet))
                    {
                        eventSnippetDump.Add(routeEvent.Snippet);
                    }

                    routeEvents.Add(routeEvent);
                }
            }

            return routeEvents;
        }

        private static void AssignEvents(
            RouteSet routeset,
            IReadOnlyDictionary<RouteNode, RouteNodeEventData> nodeEventDataTable,
            IReadOnlyList<RouteEvent> routeEvents)
        {
            var readEvents = 0;
            foreach (var route in routeset.Routes)
            {
                foreach (var node in route.Nodes)
                {
                    var eventData = nodeEventDataTable[node];
                    node.EdgeEvent = routeEvents[eventData.EdgeEventIndex];

                    node.Events = new List<RouteEvent>();
                    for (var i = 0; i < eventData.EventCount; i++)
                    {
                        var nextEvent = routeEvents[i + readEvents];
                        if (nextEvent == node.EdgeEvent)
                        {
                            continue;
                        }

                        node.Events.Add(nextEvent);
                    }

                    readEvents += eventData.EventCount;
                }
            }
        }

        private void InitializeDictionaries()
        {
            this.routeNameHashManager = new StrCode32HashManager();
            this.eventNameHashManager = new StrCode32HashManager();
            this.messageHashManager = new StrCode32HashManager();

            this.routeNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.IdDictionary);
            this.eventNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.EventDictionary);
            this.messageHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.MessageDictionary);
        }

        private static List<RouteNode> ReadNodesForRoute(ReadFloat readFloat, SetPosition setRoutePosition, SetTransformParent parentNodeToRoute, uint routeNodeCount, string routeName)
        {
            var nodes = new List<RouteNode>();
            for (var i = 0; i < routeNodeCount; i++)
            {
                var nodePosition = ReadVector3(readFloat);
                var node = CreateRouteNode(routeName, i, nodePosition);

                // Set the position of a route to the position of its first node, so that double-clicking on a route in the Hierarchy has intuitive behavior.
                if (i == 0)
                {
                    setRoutePosition(node.transform.position);
                }
                
                parentNodeToRoute(node.transform);
                nodes.Add(node);
            }
            return nodes;
        }

        private static Vector3 ReadVector3(ReadFloat readFloat)
        {
            var x = readFloat();
            var y = readFloat();
            var z = readFloat();

            // Convert Fox to Unity axes.
            return new Vector3(z, y, x);
        }

        private static RouteSet CreateRouteSet(string name)
        {
            var go = new GameObject { name = name };
            return go.AddComponent<RouteSet>();
        }

        private static Route CreateRoute(string name)
        {
            var go = new GameObject { name = name };
            return go.AddComponent<Route>();
        }

        private static RouteNode CreateRouteNode(string routeName, int nodeIndex, Vector3 position)
        {
            var nodeName = routeName + "_RouteNode" + nodeIndex.ToString("0000");
            var go = new GameObject { name = nodeName };

            var node = go.AddComponent<RouteNode>();
            node.transform.position = position;
            return node;
        }

        public struct RouteDefinition
        {
            public static uint SizeBytes => 3 * sizeof(uint) + 2 * sizeof(ushort);

            private uint NodeOffset { get; }

            private uint EdgeOffset { get; } // TODO rename

            private uint EventOffset { get; }

            public ushort NumEdgeEvents { get; }

            public ushort NumEvents { get; }

            public static RouteDefinition Read(BinaryReader input)
            {
                var nodeOffset = input.ReadUInt32();
                var edgeOffset = input.ReadUInt32();
                var eventOffset = input.ReadUInt32();
                var numEdgeEvents = input.ReadUInt16();
                var numEvents = input.ReadUInt16();

                return new RouteDefinition(nodeOffset, edgeOffset, eventOffset, numEdgeEvents, numEvents);
            }

            private RouteDefinition(
                uint nodeOffset,
                uint edgeOffset,
                uint eventOffset,
                ushort numEdgeEvents,
                ushort numEvents)
            {
                this.NodeOffset = nodeOffset;
                this.EdgeOffset = edgeOffset;
                this.EventOffset = eventOffset;
                this.NumEdgeEvents = numEdgeEvents;
                this.NumEvents = numEvents;
            }
        }

        private struct RouteNodeEventData
        {
            public short EventCount { get; }

            public short EdgeEventIndex { get; }

            public static RouteNodeEventData Read(BinaryReader input)
            {
                var nodeEventCount = input.ReadInt16();
                var edgeEventIndex = input.ReadInt16();
                return new RouteNodeEventData(nodeEventCount, edgeEventIndex);
            }

            private RouteNodeEventData(short eventCount, short edgeEventIndex)
            {
                this.EventCount = eventCount;
                this.EdgeEventIndex = edgeEventIndex;
            }
        }
    }
}