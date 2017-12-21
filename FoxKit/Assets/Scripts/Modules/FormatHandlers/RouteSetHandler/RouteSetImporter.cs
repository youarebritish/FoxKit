namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System.Collections.Generic;
    using System.IO;

    using FoxKit.Core;

    using GzsTool.Core;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> routeNameHashManager;

        private IHashManager<uint> eventNameHashManager;

        public static TextAsset RouteNameDictionary { get; set; }

        public static TextAsset EventNameDictionary { get; set; }

        public override void OnImportAsset(AssetImportContext ctx)
        {
            this.routeNameHashManager = new StrCode32HashManager();
            this.eventNameHashManager = new StrCode32HashManager();

            this.routeNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.IdDictionary);
            this.eventNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.EventDictionary);

            var path = ctx.assetPath;

            var routeset = CreateRouteSet(Path.GetFileNameWithoutExtension(path));
            routeset.transform.position = Vector3.zero;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                // Header
                // TODO: Check if GZ version
                reader.Skip(6);

                var routeIdCount = reader.ReadInt16();
                routeset.Routes = new List<Route>(routeIdCount);
                var routeIdsOffset = reader.ReadInt32();
                var routeDefinitionsOffset = reader.ReadInt32();

                reader.Skip(3 * sizeof(int));

                for (var i = 0; i < routeIdCount; i++)
                {
                    var routeNameHash = reader.ReadUInt32();
                    string routeNameString;
                    if (!this.routeNameHashManager.TryGetStringFromHash(routeNameHash, out routeNameString))
                    {
                        routeNameString = routeNameHash.ToString();
                    }

                    var route = CreateRoute(routeNameString);

                    routeset.Routes.Add(route);
                    route.transform.SetParent(routeset.transform);
                }

                // Read route definitions.
                var nodeCount = new Dictionary<Route, int>();
                var eventCount = new Dictionary<Route, int>();
                foreach (var route in routeset.Routes)
                {
                    var routeDefinition = RouteDefinition.Read(reader);
                    nodeCount.Add(route, routeDefinition.NumEdgeEvents);
                    eventCount.Add(route, routeDefinition.NumEvents);
                }

                // Read route nodes.
                foreach (var route in routeset.Routes)
                {
                    ReadNodesForRoute(reader, route, nodeCount[route]);
                }

                // Read route "edges."
                var nodeEventDataTable = new Dictionary<RouteNode, RouteNodeEventData>();
                foreach (var route in routeset.Routes)
                {
                    foreach (var node in route.Nodes)
                    {
                        var nodeEventData = RouteNodeEventData.Read(reader);
                        nodeEventDataTable.Add(node, nodeEventData);
                    }
                }

                // Read events.
                var routeEvents = new List<RouteEvent>();
                foreach (var route in eventCount.Keys)
                {
                    for (var i = 0; i < eventCount[route]; i++)
                    {
                        var eventNameHash = reader.ReadUInt32();
                        string eventNameString;
                        if (!this.eventNameHashManager.TryGetStringFromHash(eventNameHash, out eventNameString))
                        {
                            eventNameString = eventNameHash.ToString();
                        }

                        var routeEvent = new RouteEvent { Name = eventNameString };

                        for (var j = 0; j < 10; j++)
                        {
                            routeEvent.Params.Add(reader.ReadUInt32());
                        }
                        // TODO: ArgumentException: The output char buffer is too small to contain the decoded characters, encoding 'Unicode (UTF-8)' fallback 'System.Text.DecoderReplacementFallback'
                        var snippet = reader.ReadUInt32();//reader.ReadChars(4);
                        routeEvent.Snippet = snippet.ToString();//new string(snippet));
                        routeEvents.Add(routeEvent);
                    }
                }

                // Assign events to nodes.
                var readEvents = 0;
                foreach (var route in routeset.Routes)
                {
                    foreach (var node in route.Nodes)
                    {
                        var eventData = nodeEventDataTable[node];
                        node.Events = new List<RouteEvent>(eventData.EventCount);

                        for (var i = 0; i < eventData.EventCount; i++)
                        {
                            node.Events.Add(routeEvents[i + readEvents]);
                        }
                        node.EdgeEvent = routeEvents[eventData.EdgeEventIndex];
                        readEvents += eventData.EventCount;
                    }
                }
            }

            ctx.AddObjectToAsset(routeset.name, routeset.gameObject);
            ctx.SetMainObject(routeset.gameObject);
        }

        private static void ReadNodesForRoute(BinaryReader input, Route route, int nodeCount)
        {
            for (var i = 0; i < nodeCount; i++)
            {
                var x = input.ReadSingle();
                var y = input.ReadSingle();
                var z = input.ReadSingle();

                var node = CreateRouteNode(route.name, i, new Vector3(z, y, x));

                // Set the position of a route to the position of its first node, so that double-clicking on a route in the Hierarchy has intuitive behavior.
                if (i == 0)
                {
                    route.transform.position = node.transform.position;
                }

                route.Nodes.Add(node);
                node.transform.SetParent(route.transform);
            }
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

        private static RouteNode CreateRouteNode(string routeName, int index, Vector3 position)
        {
            var nodeName = routeName + "_node" + index;
            var go = new GameObject { name = nodeName };

            var node = go.AddComponent<RouteNode>();
            node.transform.position = position;
            return node;
        }

        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }

        private struct RouteDefinition
        {
            public int NodeOffset { get; private set; }

            public int EdgeOffset { get; private set; } // TODO rename

            public int EventOffset { get; private set; }

            public short NumEdgeEvents { get; private set; }

            public short NumEvents { get; private set; }

            public static RouteDefinition Read(BinaryReader input)
            {
                var nodeOffset = input.ReadInt32();
                var edgeOffset = input.ReadInt32();
                var eventOffset = input.ReadInt32();
                var numEdgeEvents = input.ReadInt16();
                var numEvents = input.ReadInt16();

                return new RouteDefinition(nodeOffset, edgeOffset, eventOffset, numEdgeEvents, numEvents);
            }

            private RouteDefinition(
                int nodeOffset,
                int edgeOffset,
                int eventOffset,
                short numEdgeEvents,
                short numEvents)
            {
                NodeOffset = nodeOffset;
                EdgeOffset = edgeOffset;
                EventOffset = eventOffset;
                NumEdgeEvents = numEdgeEvents;
                NumEvents = numEvents;
            }
        }

        private struct RouteNodeEventData
        {
            public short EventCount { get; private set; }

            public short EdgeEventIndex { get; private set; }

            public static RouteNodeEventData Read(BinaryReader input)
            {
                var nodeEventCount = input.ReadInt16();
                var edgeEventIndex = input.ReadInt16();
                return new RouteNodeEventData(nodeEventCount, edgeEventIndex);
            }

            private RouteNodeEventData(short eventCount, short edgeEventIndex)
            {
                EventCount = eventCount;
                EdgeEventIndex = edgeEventIndex;
            }
        }
    }
}