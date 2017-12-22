using System.Collections.Generic;
using System.IO;
using System.Linq;

using FoxKit.Core;
using FoxKit.Modules.FormatHandlers.RouteSetHandler;

using UnityEngine.Assertions;

public static class RouteSetExporter
{
    private const uint HeaderSizeBytes = 28;
    private const uint NodeSizeBytes = 3 * sizeof(float);
    private const uint EventTableSizeBytes = 2 * sizeof(ushort);
    private const uint EventSizeBytes = sizeof(uint) + (10 * sizeof(uint)) + (4 * sizeof(byte));

    private const ushort GzFrtVersion = 2;
    private const ushort TppFrtVersion = 3;

    public static void ExportRouteSet(RouteSet routeSet, StrCode32HashManager hashManager, string exportPath)
    {
        Assert.IsNotNull(routeSet, "RouteSet must not be null.");
        Assert.IsNotNull(hashManager, "hashManager must not be null.");
        Assert.IsNotNull(exportPath, "exportPath must not be null.");
        Assert.IsNotNull(routeSet.Routes, "RouteSet.Routes must not be null.");

        var routeCount = routeSet.Routes.Count;
        Assert.IsTrue(routeCount > 0, "Invalid route count. Cannot write a routeset with no routes.");
        Assert.IsTrue(routeCount <= ushort.MaxValue, "Invalid route count. Only up to " + ushort.MaxValue + " routes can be written to file.");

        using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
        {
            var nodes = new List<RouteNode>();
            var nodeIndices = new Dictionary<RouteNode, int>();
            CreateNodeList(routeSet, nodes, nodeIndices);

            var nodeCount = (uint)nodes.Count;
            uint routeDefinitionsOffset, nodesOffset, eventTableOffset, eventsOffset;
            WriteHeader(TppFrtVersion, (ushort)routeCount, nodeCount, writer, out routeDefinitionsOffset, out nodesOffset, out eventTableOffset, out eventsOffset);

            var routeIds = routeSet.Routes.Select(route => hashManager.GetHash(route.name)).ToList();
            WriteRouteIds(routeIds, writer);
            
            WriteRouteDefinitions(
                routeSet.Routes,
                routeDefinitionsOffset,
                nodesOffset,
                eventTableOffset,
                eventsOffset,
                nodeIndices,
                writer);

            WriteNodes(nodes, writer);
            
            var eventIndices = new Dictionary<RouteEvent, ushort>();
            BuildEventList(nodes, eventIndices);
            WriteEventTable(nodes, eventIndices, writer);

            var events = new List<RouteEvent>();
            foreach (var node in nodes)
            {
                events.Add(node.EdgeEvent);
                events.AddRange(node.Events);
            }
            WriteEvents(events, hashManager, writer);
        }
    }

    private static void BuildEventList(List<RouteNode> nodes, Dictionary<RouteEvent, ushort> eventIndices)
    {
        ushort i = 0;
        foreach (var node in nodes)
        {
            var isEdgeEventADuplicate = false;
            var duplicatedEventIndex = ushort.MaxValue;
            foreach (var routeEvent in eventIndices.Keys)
            {
                if (routeEvent != node.EdgeEvent)
                {
                    continue;
                }
                duplicatedEventIndex = eventIndices[routeEvent];
                isEdgeEventADuplicate = true;
            }
            if (!isEdgeEventADuplicate)
            {
                eventIndices.Add(node.EdgeEvent, i);
                i++;
            }
            else
            {
                eventIndices.Add(node.EdgeEvent, duplicatedEventIndex);
            }
            foreach (var @event in node.Events)
            {
                eventIndices.Add(@event, i);
                i++;
            }
        }
    }

    private static void CreateNodeList(RouteSet routeSet, List<RouteNode> nodes, Dictionary<RouteNode, int> nodeIndices)
    {
        var i = 0;
        foreach (var route in routeSet.Routes)
        {
            Assert.IsTrue(route.Nodes.Count > 0, "Route " + route.name + " has no nodes. All routes must contain at least one RouteNode.");

            foreach (var node in route.Nodes)
            {
                nodes.Add(node);
                nodeIndices.Add(node, i);
                i++;
            }
        }
    }

    private static void WriteHeader(ushort version, ushort routeCount, uint nodeCount, BinaryWriter writer, out uint routeDefinitionsOffset, out uint nodesOffset, out uint eventTableOffset, out uint eventsOffset)
    {
        writer.Write('R');
        writer.Write('O');
        writer.Write('U');
        writer.Write('T');

        writer.Write(version);
        writer.Write((ushort)routeCount);
        
        writer.Write(CalculateRouteIdsOffset());

        routeDefinitionsOffset = CalculateRouteDefinitionsOffset(HeaderSizeBytes, routeCount);
        writer.Write(routeDefinitionsOffset);

        nodesOffset = CalculateNodesOffset(routeDefinitionsOffset, routeCount);
        writer.Write(nodesOffset);
        
        eventTableOffset = CalculateEventTableOffset(nodesOffset, nodeCount);
        writer.Write(eventTableOffset);

        eventsOffset = CalculateEventsOffset(eventTableOffset, nodeCount);
        writer.Write(eventsOffset);
    }

    private static void WriteRouteIds(List<uint> routeIds, BinaryWriter writer)
    {
        foreach (var routeId in routeIds)
        {
            writer.Write(routeId);
        }
    }

    private static void WriteRouteDefinitions(List<Route> routes, uint routeDefinitionsOffset, uint nodesOffset, uint eventTableOffset, uint eventsOffset, Dictionary<RouteNode, int> nodeIndices, BinaryWriter writer)
    {
        var currentOffset = routeDefinitionsOffset;
        foreach (var route in routes)
        {
            WriteRouteDefinition(route, currentOffset, nodesOffset, eventTableOffset, eventsOffset, nodeIndices, writer);
            currentOffset += RouteSetImporter.RouteDefinition.SizeBytes;
        }
    }

    private static void WriteRouteDefinition(Route route, uint entryOffset, uint nodesOffset, uint eventTableOffset, uint eventsOffset, Dictionary<RouteNode, int> nodeIndices, BinaryWriter writer)
    {
        var initialNode = route.Nodes[0];
        var initialNodeIndex = nodeIndices[initialNode];
        var nodeOffset = (uint)(nodesOffset + (NodeSizeBytes * initialNodeIndex));
        writer.Write(nodeOffset - entryOffset);

        var eventTableEntryOffset = (uint)(eventTableOffset + (EventTableSizeBytes * initialNodeIndex));
        writer.Write(eventTableEntryOffset - entryOffset);
        
        var eventOffset = (uint)(eventsOffset + (EventSizeBytes * initialNodeIndex));

        writer.Write(eventOffset - entryOffset);
        writer.Write((ushort)route.Nodes.Count);

        // TODO: Combine duplicate events.
        var eventCount = (ushort)route.Nodes.Aggregate<RouteNode, uint>(0, (current, node) => current + (uint)node.Events.Count);
        eventCount += (ushort)route.Nodes.Count;
        writer.Write(eventCount);
    }

    private static void WriteNodes(List<RouteNode> nodes, BinaryWriter writer)
    {
        foreach (var node in nodes)
        {
            WriteNode(node, writer);
        }
    }

    private static void WriteNode(RouteNode node, BinaryWriter writer)
    {
        writer.Write(node.transform.position.z);
        writer.Write(node.transform.position.y);
        writer.Write(node.transform.position.x);
    }

    private static void WriteEventTable(List<RouteNode> nodes, IReadOnlyDictionary<RouteEvent, ushort> eventIndices, BinaryWriter writer)
    {
        foreach (var node in nodes)
        {
            WriteEventTableEntry(node, eventIndices, writer);
        }
    }

    private static void WriteEventTableEntry(RouteNode node, IReadOnlyDictionary<RouteEvent, ushort> eventIndices, BinaryWriter writer)
    {
        var eventCount = (ushort)(node.Events.Count + 1);
        writer.Write(eventCount);

        var edgeEventIndex = eventIndices[node.EdgeEvent];
        writer.Write(edgeEventIndex);
    }

    private static void WriteEvents(List<RouteEvent> events, StrCode32HashManager hashManager, BinaryWriter writer)
    {
        foreach (var @event in events)
        {
            WriteEvent(@event, hashManager, writer);
        }
    }

    private static void WriteEvent(RouteEvent @event, StrCode32HashManager hashManager, BinaryWriter writer)
    {
        var eventNameHash = hashManager.GetHash(@event.Name);
        writer.Write(eventNameHash);

        for (var i = 0; i < 10; i++)
        {
            writer.Write(@event.Params[i]);
        }

        var snippet = @event.Snippet.ToCharArray();
        for (var i = 0; i < 4; i++)
        {
            if (i >= snippet.Length)
            {
                writer.Write('\0');
            }
            else
            {
                writer.Write(snippet[i]);
            }
        }
    }

    private static uint CalculateRouteIdsOffset()
    {
        return HeaderSizeBytes;
    }

    private static uint CalculateRouteDefinitionsOffset(uint headerOffset, ushort routeCount)
    {
        return (uint)(headerOffset + (routeCount * sizeof(uint)));
    }

    private static uint CalculateNodesOffset(uint routeDefinitionsOffset, ushort routeCount)
    {
        return (uint)(routeDefinitionsOffset + (routeCount * RouteSetImporter.RouteDefinition.SizeBytes));
    }

    private static uint CalculateEventTableOffset(uint nodesOffset, uint nodeCount)
    {
        return (uint)(nodesOffset + (nodeCount * NodeSizeBytes));
    }

    private static uint CalculateEventsOffset(uint eventTableOffset, uint nodeCount)
    {
        return (uint)(eventTableOffset + (nodeCount * EventTableSizeBytes));
    }
}
