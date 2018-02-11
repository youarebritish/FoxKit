namespace FoxKit.Modules.RouteBuilder.Importer
{
    using System.IO;

    using FoxKit.Core;

    using UnityEditor.Experimental.AssetImporters;

    using UnityEngine;
    using System;
    using FoxKit.Modules.FormatHandlers.RouteSetHandler;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> routeNameHashManager;
        private IHashManager<uint> eventNameHashManager;
        private IHashManager<uint> messageHashManager;

        public delegate TryUnhashResult TryUnhashDelegate(uint hash);

        private delegate RouteNode CreateNodeGameObjectDelegate(RouteSet routeSet, Route owner, FoxLib.Tpp.RouteSet.RouteNode node, string nodeName);
        private delegate Route CreateRouteGameObjectDelegate(RouteSet owner, FoxLib.Tpp.RouteSet.Route route);
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

            var buildEvent = EventFactory.CreateFactory(getEventName);
            var buildNode = NodeFactory.CreateFactory(buildEvent);
            var buildRoute = RouteFactory.CreateFactory(buildNode, getRouteName, CreateNodeName);
            var buildRouteSet = RouteSetFactory.CreateFactory(buildRoute);

            var routeSetGameObject = buildRouteSet.Invoke(routeSet, Path.GetFileNameWithoutExtension(ctx.assetPath));

            ctx.AddObjectToAsset(routeSetGameObject.name, routeSetGameObject.gameObject);
            ctx.SetMainObject(routeSetGameObject.gameObject);
        }

        private static TryUnhashDelegate MakeUnhashFunc(IHashManager<uint> hashManager)
        {
            return (hash =>
            {
                string result;
                if (!hashManager.TryGetStringFromHash(hash, out result))
                {
                    return new TryUnhashResult(hash);
                }
                return new TryUnhashResult(result);
            });
        }

        private static string CreateNodeName(string routeName, int nodeIndex)
        {
            return String.Format("{0}_Node{1:0000}", routeName, nodeIndex.ToString("0000"));
        }

        public class TryUnhashResult
        {
            public bool WasNameUnhashed { get; }
            public string UnhashedString { get; }
            public uint Hash { get; }

            public TryUnhashResult(string unhashedName)
            {
                WasNameUnhashed = true;
                UnhashedString = unhashedName;
                Hash = uint.MaxValue;
            }

            public TryUnhashResult(uint hash)
            {
                WasNameUnhashed = false;
                UnhashedString = null;
                Hash = hash;
            }
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