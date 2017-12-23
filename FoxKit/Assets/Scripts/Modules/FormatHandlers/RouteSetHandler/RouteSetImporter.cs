namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using FoxKit.Core;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> eventNameHashManager;

        private IHashManager<uint> messageHashManager;

        private IHashManager<uint> routeNameHashManager;

        private const long RouteIdsOffset = 28;

        public const ushort RouteDefinitionSizeBytes = (3 * sizeof(uint)) + (2 * sizeof(ushort));

        private delegate uint ReadUInt(BinaryReader reader, long position);

        private delegate ushort ReadUShort(BinaryReader reader, long position);

        private delegate float ReadFloat(BinaryReader reader, long position);

        private delegate long GetRouteNodeCountOffset(ushort routeIndex, long routeDefinitionsOffset);

        private delegate long GetRouteEventCountOffset(ushort routeIndex, long routeDefinitionsOffset);

        private delegate long GetNodePositionsOffset(ushort routeCount, long routeDefinitionsOffset);

        private delegate long GetNodePositionOffset(ushort nodeIndex, long nodePositionsOffset);
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            this.InitializeDictionaries();

            var idHashDump = RouteSetImporterPreferences.Instance.IdHashDump.GetUniqueLines();
            var eventNameHashDump = RouteSetImporterPreferences.Instance.EventHashDump.GetUniqueLines();
            var eventSnippetDump = RouteSetImporterPreferences.Instance.JsonDump.GetUniqueLines();

            var path = ctx.assetPath;
            var routesetName = Path.GetFileNameWithoutExtension(path);
            var routeset = ReadRouteSet(
                ctx.assetPath,
                routesetName,
                this.ReadUShortFromPosition,
                this.ReadUIntFromPosition,
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
            string assetPath,
            string routesetName,
            ReadUShort readUShortFromPosition,
            ReadUInt readUIntFromPosition,
            ReadFloat readFloatFromPosition,
            IHashManager<uint> routeNameHashManager,
            IHashManager<uint> eventNameHashManager,
            ISet<string> idHashDump,
            ISet<string> eventNameHashDump,
            ISet<string> eventSnippetDump)
        {
            var routeset = CreateRouteSet(routesetName);

            using (var reader = new BinaryReader(new FileStream(assetPath, FileMode.Open)))
            {
                var version = ReadVersion(reader, readUShortFromPosition);
                if (version != 3)
                {
                    throw new InvalidDataException("Frt version " + version + " is not supported.");
                }

                var routeCount = ReadRouteCount(reader, readUShortFromPosition);
                var routeNames = ReadRouteNames(reader, routeCount, readUIntFromPosition, routeNameHashManager, idHashDump);
                ushort eventCount = 0;

                for (ushort routeIndex = 0; routeIndex < routeNames.Count; routeIndex++)
                {
                    var routeName = routeNames[routeIndex];
                    var route = CreateRoute(routeName, routeset.transform);

                    var routeNodeCount = ReadRouteNodeCount(
                        reader,
                        routeIndex,
                        routeCount,
                        CalculateRouteNodeCountOffset,
                        readUShortFromPosition);

                    var routeDefinitionsOffset = CalculateRouteDefinitionsOffset(routeCount);
                    var nodePositionsOffset = CalculateNodePositionsOffset(routeCount, routeDefinitionsOffset);

                    route.Nodes = ReadNodesForRoute(
                        reader,
                        routeNodeCount,
                        routeName,
                        readFloatFromPosition,
                        CalculateNodePositionOffset,
                        nodePositionsOffset);

                    var routeEventCount = ReadRouteEventCount(
                        reader,
                        routeIndex,
                        routeCount,
                        CalculateRouteNodeCountOffset,
                        readUShortFromPosition);
                    eventCount += routeEventCount;

                    route.transform.SetParent(routeset.transform);
                    routeset.Routes.Add(route);
                }

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

        private static List<string> ReadRouteNames(
            BinaryReader reader,
            ushort routeCount,
            ReadUInt readUIntFromPosition,
            IHashManager<uint> routeNameHashManager,
            ISet<string> unmatchedRouteIdHashes)
        {
            var routeNames = new List<string>(routeCount);

            for (uint i = 0; i < routeCount; i++)
            {
                var routeIdHash = ReadRouteIdHash(reader, readUIntFromPosition, i);
                var routeName = ReadRouteName(routeIdHash, routeNameHashManager, unmatchedRouteIdHashes);
                routeNames.Add(routeName);
            }

            return routeNames;
        }

        private static ushort ReadRouteCount(BinaryReader reader, ReadUShort readUShortFromPosition)
        {
            const long RouteIdCountOffset = 6;
            var routeIdCount = readUShortFromPosition(reader, RouteIdCountOffset);
            return routeIdCount;
        }

        private static ushort ReadVersion(BinaryReader reader, ReadUShort readUShortFromPosition)
        {
            const long VersionOffset = 4;
            var version = readUShortFromPosition(reader, VersionOffset);
            return version;
        }

        private ReadUInt ReadUIntFromPosition
        {
            get
            {
                ReadUInt readUint32 = delegate(BinaryReader binaryReader, long position)
                    {
                        binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
                        return binaryReader.ReadUInt32();
                    };
                return readUint32;
            }
        }

        private ReadUShort ReadUShortFromPosition
        {
            get
            {
                ReadUShort readUShort = delegate (BinaryReader binaryReader, long position)
                    {
                        binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
                        return binaryReader.ReadUInt16();
                    };
                return readUShort;
            }
        }

        private static uint ReadRouteIdHash(BinaryReader reader, ReadUInt readUint32, uint routeIndex)
        {
            const uint RouteIdsOffset = 28;
            var readerPosition = GetRouteIdOffset(RouteIdsOffset, routeIndex);
            return readUint32(reader, readerPosition);
        }

        private static uint GetRouteIdOffset(uint routeIdsOffset, uint routeIndex)
        {
            var offset = routeIdsOffset + (routeIndex * sizeof(uint));
            return offset;
        }
        
        private static string ReadRouteName(
            uint routeIdHash,
            IHashManager<uint> routeNameHashManager,
            ISet<string> unmatchedRouteIdHashes)
        {
            string routeName;
            if (!routeNameHashManager.TryGetStringFromHash(routeIdHash, out routeName))
            {
                var routeHashString = routeIdHash.ToString();
                if (!unmatchedRouteIdHashes.Contains(routeHashString))
                {
                    unmatchedRouteIdHashes.Add(routeHashString);
                }
            }
            
            return routeName;
        }
        
        private static ushort ReadRouteNodeCount(BinaryReader reader, ushort routeIndex, ushort routeIdCount, GetRouteNodeCountOffset getRouteNodeCountOffset, ReadUShort readUShort)
        {
            var offset = getRouteNodeCountOffset(routeIndex, routeIdCount);
            return readUShort(reader, offset);
        }

        private static ushort ReadRouteEventCount(BinaryReader reader, ushort routeIndex, ushort routeIdCount, GetRouteEventCountOffset getRouteEventCountOffset, ReadUShort readUShort)
        {
            var offset = getRouteEventCountOffset(routeIndex, routeIdCount);
            return readUShort(reader, offset);
        }

        private static long CalculateRouteNodeCountOffset(ushort routeIndex, long routeDefinitionsOffset)
        {
            var routeDefinitionOffset = CalculateRouteDefinitionOffset(routeIndex, routeDefinitionsOffset);
            var routeNodeCountOffset = routeDefinitionOffset + (3 * sizeof(uint));
            return routeNodeCountOffset;
        }

        private static long CalculateRouteEventCountOffset(ushort routeIndex, long routeDefinitionsOffset)
        {
            var routeDefinitionOffset = CalculateRouteDefinitionOffset(routeIndex, routeDefinitionsOffset);
            var routeNodeCountOffset = routeDefinitionOffset + (3 * sizeof(uint)) + sizeof(ushort);
            return routeNodeCountOffset;
        }

        private static long CalculateRouteDefinitionsOffset(ushort routeCount)
        {
            var routeDefinitionsOffset = RouteIdsOffset + (routeCount * sizeof(uint));
            return routeDefinitionsOffset;
        }
        
        private static long CalculateRouteDefinitionOffset(ushort routeIndex, long routeDefinitionsOffset)
        {
            var routeDefinitionOffset = routeDefinitionsOffset + (routeIndex * RouteDefinitionSizeBytes);
            return routeDefinitionOffset;
        }
        
        private static long CalculateNodePositionsOffset(ushort routeCount, long routeDefinitionsOffset)
        {
            return routeDefinitionsOffset + (routeCount * RouteDefinitionSizeBytes);
        }

        private static long CalculateNodePositionOffset(ushort nodeIndex, long nodePositionsOffset)
        {
            const ushort NodePositionSizeBytes = 3 * sizeof(float);
            return nodePositionsOffset + (nodeIndex * NodePositionSizeBytes);
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
            ushort eventCount,
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

        private static List<RouteNode> ReadNodesForRoute(BinaryReader reader, uint routeNodeCount, string routeName, ReadFloat readFloat, GetNodePositionOffset getNodePositionOffset, long nodePositionsOffset)
        {
            var nodes = new List<RouteNode>();
            for (ushort i = 0; i < routeNodeCount; i++)
            {
                var nodeOffset = getNodePositionOffset(i, nodePositionsOffset);
                var nodePosition = ReadNodePosition(reader, nodeOffset, readFloat);
                var node = CreateRouteNode(routeName, i, nodePosition);
                nodes.Add(node);
            }
            return nodes;
        }

        private static Vector3 ReadNodePosition(BinaryReader reader, long nodeOffset, ReadFloat readFloat)
        {
            var x = readFloat(reader, nodeOffset);
            var y = readFloat(reader, nodeOffset + sizeof(float));
            var z = readFloat(reader, nodeOffset + (2 * sizeof(float)));

            // Convert Fox to Unity axes.
            return new Vector3(z, y, x);
        }

        private static RouteSet CreateRouteSet(string name)
        {
            var go = new GameObject { name = name };
            return go.AddComponent<RouteSet>();
        }

        private static Route CreateRoute(string name, Transform parent)
        {
            var go = new GameObject { name = name };
            go.transform.SetParent(parent);
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

        /*public struct RouteDefinition
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
        }*/

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