using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using FoxKit.Core;

using GzsTool.Core;

using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

[ScriptedImporter(1, "frt")]
public class RouteSetHandler : ScriptedImporter, IFormatHandler
{
    public List<string> Extensions => new List<string> { "frt" };

    private List<uint> routeNames = new List<uint>();

    private HashSet<uint> eventNames = new HashSet<uint>();

    private HashSet<uint> messageNames = new HashSet<uint>();

    private HashSet<string> jsonSnippets = new HashSet<string>();

    public RouteSetHandler()
    {
        
    }

    public RouteSetHandler(List<uint> routeNames, HashSet<uint> eventNames, HashSet<uint> messageNames, HashSet<string> jsonSnippets)
    {
        this.routeNames = routeNames;
        this.eventNames = eventNames;
        this.messageNames = messageNames;
        this.jsonSnippets = jsonSnippets;
    }

    public object Import(Stream input, string path)
    {
        var routeset = CreateRouteSet(Path.GetFileNameWithoutExtension(path));
        routeset.transform.position = Vector3.zero;

        using (var reader = new BinaryReader(input))
        {
            // Header
            reader.Skip(6);
            
            var routeIdCount = reader.ReadInt16();
            routeset.Routes = new List<Route>(routeIdCount);
            var routeIdsOffset = reader.ReadInt32();
            var routeDefinitionsOffset = reader.ReadInt32();

            reader.Skip(3 * sizeof(int));
            
            for (var i = 0; i < routeIdCount; i++)
            {
                var routeName = reader.ReadUInt32(); // TODO: Unhash
                routeNames.Add(routeName);
                var route = CreateRoute(routeName.ToString());

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
                    var eventName = reader.ReadUInt32();
                    this.eventNames.Add(eventName);

                    var routeEvent = new RouteEvent { Name = eventName.ToString() };

                    for (var j = 0; j < 10; j++)
                    {
                        routeEvent.Params.Add(reader.ReadUInt32());
                    }
                    //var jsonSnippet = reader.ReadChars(4);
                    routeEvent.Params.Add(0); // TODO
                    if (!routeEvents.Contains(routeEvent))
                    {
                        routeEvents.Add(routeEvent);
                    }

                    if (eventName == 2265318157)
                    {
                        // SendMessage
                        if (!this.messageNames.Contains(routeEvent.Params[7]))
                        {
                            this.messageNames.Add(routeEvent.Params[7]);
                        }
                    }

                    // Json snippets
                    /*if (!this.jsonSnippets.Contains(jsonSnippet.ToString()))
                    {
                        this.jsonSnippets.Add(jsonSnippet.ToString());
                    }*/
                }
            }

            // Assign events to nodes.
            foreach (var route in routeset.Routes)
            {
                foreach (var node in route.Nodes)
                {
                    var eventData = nodeEventDataTable[node];
                    node.Events = new List<RouteEvent>(eventData.EventCount);

                    for (var i = 0; i < eventData.EventCount; i++)
                    {
                        node.Events.Add(routeEvents[i]);
                    }
                    node.EdgeEvent = routeEvents[eventData.EdgeEventIndex];
                }
            }
        }

        // FIXME: Save to prefab!
        /*foreach (var route in routeset.Routes)
        {
            foreach (var node in route.Nodes)
            {
                GameObject.DestroyImmediate(node.gameObject);
            }
            GameObject.DestroyImmediate(route.gameObject);
        }
        GameObject.DestroyImmediate(routeset.gameObject);*/
        return routeset;
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
        node.Position = position;
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

        private RouteDefinition(int nodeOffset, int edgeOffset, int eventOffset, short numEdgeEvents, short numEvents)
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

    public override void OnImportAsset(AssetImportContext ctx)
    {
        using (var inputStream = new FileStream(ctx.assetPath, FileMode.Open))
        {
            var routeset = Import(inputStream, ctx.assetPath) as RouteSet;
            ctx.AddObjectToAsset(routeset.name, routeset.gameObject);
            ctx.SetMainObject(routeset.gameObject);
        }
    }
}
