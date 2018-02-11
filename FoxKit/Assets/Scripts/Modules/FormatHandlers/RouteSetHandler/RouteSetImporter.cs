namespace FoxKit.Modules.FormatHandlers.RouteSetHandler
{
    using System.IO;

    using FoxKit.Core;

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

        private delegate RouteNode CreateNodeGameObjectDelegate(Transform parent, FoxLib.Tpp.RouteSet.RouteNode node, string nodeName);
        private delegate Route CreateRouteGameObjectDelegate(FoxLib.Tpp.RouteSet.Route route);
        private delegate RouteEvent CreateRouteEventComponentDelegate(GameObject parent, FoxLib.Tpp.RouteSet.RouteEvent @event);

        public override void OnImportAsset(AssetImportContext ctx)
        {
            InitializeDictionaries();

            FoxLib.Tpp.RouteSet.RouteSet routeSet = null;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open), FoxLib.Tpp.RouteSet.getEncoding()))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                var readFunctions = new FoxLib.Tpp.RouteSet.ReadFunctions(reader.ReadSingle, reader.ReadUInt16, reader.ReadUInt32, reader.ReadInt32, reader.ReadBytes, skipBytes);
                routeSet = FoxLib.Tpp.RouteSet.Read(readFunctions);
            }

            var getRouteName = MakeUnhashFunc(routeNameHashManager);
            var getEventName = MakeUnhashFunc(eventNameHashManager);

            var buildEvent = MakeEventBuilderDelegate(getEventName);
            var buildNode = MakeNodeBuilderDelegate(buildEvent);
            var buildRoute = MakeRouteBuilderDelegate(getRouteName, buildNode);

            var routeSetGameObject = CreateRouteSetGameObject(routeSet, Path.GetFileNameWithoutExtension(ctx.assetPath), getRouteName, buildRoute);

            ctx.AddObjectToAsset(routeSetGameObject.name, routeSetGameObject.gameObject);
            ctx.SetMainObject(routeSetGameObject.gameObject);
        }

        private static CreateNodeGameObjectDelegate MakeNodeBuilderDelegate(CreateRouteEventComponentDelegate createEvent)
        {
            return (parent, node, nodeName) => CreateRouteNodeGameObject(parent, node, nodeName, createEvent);
        }

        private static CreateRouteGameObjectDelegate MakeRouteBuilderDelegate(TryUnhashDelegate getRouteName, CreateNodeGameObjectDelegate createNode)
        {
            return route => CreateRouteGameObject(route, getRouteName, createNode);
        }

        private static CreateRouteEventComponentDelegate MakeEventBuilderDelegate(TryUnhashDelegate getEventName)
        {
            return (parent, @event) => CreateRouteEventComponent(parent, @event, getEventName);
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

        private static RouteSet CreateRouteSetGameObject(FoxLib.Tpp.RouteSet.RouteSet routeSet, string routeSetName, TryUnhashDelegate getRouteName, CreateRouteGameObjectDelegate createRoute)
        {
            var gameObject = new GameObject(routeSetName);
            var routeSetComponent = gameObject.AddComponent<RouteSet>();

            routeSetComponent.Routes = (from route in routeSet.Routes
                                       select createRoute(route))
                                       .ToList();

            foreach(var routeGameObject in routeSetComponent.Routes)
            {
                routeGameObject.transform.SetParent(gameObject.transform);
            }

            return routeSetComponent;
        }

        private static Route CreateRouteGameObject(FoxLib.Tpp.RouteSet.Route route, TryUnhashDelegate getRouteName, CreateNodeGameObjectDelegate createNode)
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
                                        createNode.Invoke(gameObject.transform, node, CreateNodeName(gameObject.name, index)))
                                    .ToList();
            
            return routeComponent;
        }

        private static string CreateNodeName(string routeName, int nodeIndex)
        {
            return String.Format("{0}_Node{1:0000}", routeName, nodeIndex.ToString("0000"));
        }

        private static RouteNode CreateRouteNodeGameObject(Transform parent, FoxLib.Tpp.RouteSet.RouteNode node, string nodeName, CreateRouteEventComponentDelegate createEvent)
        {
            var gameObject = new GameObject(nodeName);
            var nodeComponent = gameObject.AddComponent<RouteNode>();
            gameObject.transform.position = FoxUtils.FoxToUnity(node.Position);
            gameObject.transform.SetParent(parent);

            // TODO: Handle instancing; put these on the RouteSet
            nodeComponent.EdgeEvent = createEvent.Invoke(gameObject, node.EdgeEvent);
            nodeComponent.Events = (from @event in node.Events
                                    select createEvent.Invoke(gameObject, @event))
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
                
        private void InitializeDictionaries()
        {
            this.routeNameHashManager = new StrCode32HashManager();
            this.eventNameHashManager = new StrCode32HashManager();
            this.messageHashManager = new StrCode32HashManager();

            this.routeNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.IdDictionary);
            this.eventNameHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.EventDictionary);
            this.messageHashManager.LoadDictionary(RouteSetImporterPreferences.Instance.MessageDictionary);
        }
    }
}