namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System.Collections.Generic;
    using System.IO;

    using FoxKit.Core;

    using GzsTool.Core;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;
    using System;
    using System.Linq;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> routeNameHashManager;
        private IHashManager<uint> eventNameHashManager;
        private IHashManager<uint> messageHashManager;

        private delegate UnhashNameResult TryUnhashDelegate(uint hash);

        public override void OnImportAsset(AssetImportContext ctx)
        {
            this.InitializeDictionaries();            

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open), FoxLib.Tpp.RouteSet.getEncoding()))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                var readFunctions = new FoxLib.Tpp.RouteSet.ReadFunctions(reader.ReadSingle, reader.ReadUInt16, reader.ReadUInt32, reader.ReadInt32, reader.ReadBytes, skipBytes);
                var routeSet = FoxLib.Tpp.RouteSet.Read(readFunctions);

                var routeSetGameObject = CreateRouteSetGameObject(routeSet, Path.GetFileNameWithoutExtension(ctx.assetPath), MakeUnhashFunc(routeNameHashManager), MakeUnhashFunc(eventNameHashManager));
                ctx.AddObjectToAsset(routeSetGameObject.name, routeSetGameObject.gameObject);
                ctx.SetMainObject(routeSetGameObject.gameObject);
            }            
        }

        private static TryUnhashDelegate MakeUnhashFunc(IHashManager<uint> hashManager)
        {
            return (hash =>
            {
                string result;
                if (!hashManager.TryGetStringFromHash(hash, out result))
                {
                    return new UnhashNameResult(hash);
                }
                return new UnhashNameResult(result);
            });
        }

        private class UnhashNameResult
        {
            public bool WasNameUnhashed { get; }
            public string UnhashedString { get; }
            public uint Hash { get; }

            public UnhashNameResult(string unhashedName)
            {
                WasNameUnhashed = true;
                UnhashedString = unhashedName;
                Hash = uint.MaxValue;
            }

            public UnhashNameResult(uint hash)
            {
                WasNameUnhashed = false;
                UnhashedString = null;
                Hash = hash;
            }
        }

        private static RouteSet CreateRouteSetGameObject(FoxLib.Tpp.RouteSet.RouteSet routeSet, string routeSetName, TryUnhashDelegate getRouteName, TryUnhashDelegate getEventName)
        {
            var gameObject = new GameObject(routeSetName);
            var routeSetComponent = gameObject.AddComponent<RouteSet>();

            var routeGameObjects = from route in routeSet.Routes
                                   select CreateRouteGameObject(route, getRouteName, getEventName);

            foreach(var routeGameObject in routeGameObjects)
            {
                routeGameObject.transform.SetParent(gameObject.transform);
            }

            return routeSetComponent;
        }

        private static Route CreateRouteGameObject(FoxLib.Tpp.RouteSet.Route route, TryUnhashDelegate getRouteName, TryUnhashDelegate getEventName)
        {
            var gameObject = new GameObject();
            var routeComponent = gameObject.AddComponent<Route>();

            var routeNameContainer = getRouteName(route.Name);
            if (routeNameContainer.WasNameUnhashed)
            {
                gameObject.name = routeNameContainer.UnhashedString;
            }
            else
            {
                gameObject.name = routeNameContainer.Hash.ToString();
                routeComponent.TreatNameAsHash = true;
            }            
            
            routeComponent.Nodes = route.Nodes
                                    .Select((node, index) => 
                                        CreateRouteNodeGameObject(gameObject.transform, node, CreateNodeName(gameObject.name, index), getEventName))
                                    .ToList();
            
            return routeComponent;
        }

        private static string CreateNodeName(string routeName, int nodeIndex)
        {
            return String.Format("{0}_Node{1:0000}", routeName, nodeIndex.ToString("0000"));
        }

        private static RouteNode CreateRouteNodeGameObject(Transform parent, FoxLib.Tpp.RouteSet.RouteNode node, string nodeName, TryUnhashDelegate getEventName)
        {
            var gameObject = new GameObject(nodeName);
            var nodeComponent = gameObject.AddComponent<RouteNode>();
            gameObject.transform.position = FoxUtils.FoxToUnity(node.Position);
            gameObject.transform.SetParent(parent);

            // TODO: Handle instancing; put these on the RouteSet
            nodeComponent.EdgeEvent = CreateRouteEventComponent(gameObject, node.EdgeEvent, getEventName);
            nodeComponent.Events = (from @event in node.Events
                                    select CreateRouteEventComponent(gameObject, @event, getEventName))
                                   .ToList();

            return nodeComponent;
        }

        private static RouteEvent CreateRouteEventComponent(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent @event, TryUnhashDelegate getEventName)
        {
            var component = parent.AddComponent<RouteEvent>();

            var eventNameContainer = getEventName(@event.EventType);
            if (eventNameContainer.WasNameUnhashed)
            {
                component.Type = eventNameContainer.UnhashedString;
            }
            else
            {
                component.Type = eventNameContainer.Hash.ToString();
            }
            
            component.Params = @event.Params.Cast<uint>().ToList();
            return component;
        }

        /// <summary>
        /// Skip reading a number of bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="numberOfBytes">The number of bytes to skip.</param>
        private static void SkipBytes(BinaryReader reader, int numberOfBytes)
        {
            reader.BaseStream.Position += numberOfBytes;
        }

        private static RouteSet ReadRouteSet(AssetImportContext ctx, string routesetName, IHashManager<uint> routeNameHashManager, IHashManager<uint> eventNameHashManager, ISet<string> idHashDump, ISet<string> eventNameHashDump, ISet<string> eventSnippetDump)
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

                var nodeCount = new Dictionary<Route, int>();
                var eventCount = new Dictionary<Route, int>();
                ReadRouteDefinitions(routeset, reader, nodeCount, eventCount);

                ReadNodes(routeset, reader, nodeCount);

                var eventTable = ReadEventTable(routeset, reader);
                var routeEvents = ReadEvents(eventCount, reader, eventNameHashManager, eventNameHashDump, eventSnippetDump);
                AssignEvents(routeset, eventTable, routeEvents);
            }

            return routeset;
        }

        private static List<Route> ReadHeader(BinaryReader reader, IHashManager<uint> routeNameHashManager, ISet<string> idHashDump)
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

        private static Route ReadRoute(BinaryReader reader, IHashManager<uint> routeNameHashManager, ISet<string> idHashDump)
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
            IDictionary<Route, int> nodeCount,
            IDictionary<Route, int> eventCount)
        {
            foreach (var route in routeset.Routes)
            {
                var routeDefinition = RouteDefinition.Read(reader);
                nodeCount.Add(route, routeDefinition.NumEdgeEvents);
                eventCount.Add(route, routeDefinition.NumEvents);
            }
        }

        private static void ReadNodes(RouteSet routeset, BinaryReader reader, IReadOnlyDictionary<Route, int> nodeCount)
        {
            foreach (var route in routeset.Routes)
            {
                ReadNodesForRoute(reader, route, nodeCount[route]);
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

        private static List<RouteEvent> ReadEvents(Dictionary<Route, int> eventCount, BinaryReader reader, IHashManager<uint> eventNameHashManager, ISet<string> eventNameHashDump, ISet<string> eventSnippetDump)
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

                    var routeEvent = new RouteEvent { Type = eventNameString };

                    for (var j = 0; j < 10; j++)
                    {
                        routeEvent.Params.Add(reader.ReadUInt32());
                    }

                    var snippet = reader.ReadBytes(4);
                    routeEvent.Snippet = System.Text.Encoding.Default.GetString(snippet);
                    if (routeEvent.Snippet[0] == '\0' && !eventSnippetDump.Contains(routeEvent.Snippet))
                    {
                        eventSnippetDump.Add(routeEvent.Snippet);
                    }

                    routeEvents.Add(routeEvent);
                }
            }
            return routeEvents;
        }

        private static void AssignEvents(RouteSet routeset, IReadOnlyDictionary<RouteNode, RouteNodeEventData> nodeEventDataTable, IReadOnlyList<RouteEvent> routeEvents)
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
            var nodeName = routeName + "_RouteNode" + index.ToString("0000");
            var go = new GameObject { name = nodeName };

            var node = go.AddComponent<RouteNode>();
            node.transform.position = position;
            return node;
        }

        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }

        public struct RouteDefinition
        {
            public static uint SizeBytes => (3 * sizeof(uint) + 2 * sizeof(ushort));

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