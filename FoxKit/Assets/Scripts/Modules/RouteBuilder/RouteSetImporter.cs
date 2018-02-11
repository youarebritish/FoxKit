namespace FoxKit.Modules.RouteBuilder.Importer
{
    using System.IO;

    using FoxKit.Core;

    using UnityEditor.Experimental.AssetImporters;
    using System;
    using FoxKit.Modules.FormatHandlers.RouteSetHandler;

    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        private IHashManager<uint> routeNameHashManager;
        private IHashManager<uint> eventNameHashManager;
        private IHashManager<uint> messageHashManager;
        
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

            var getRouteName = routeNameHashManager.MakeUnhashFunc();
            var getEventTypeName = eventNameHashManager.MakeUnhashFunc();

            var eventIdGenerator = new EventIdGenerator();
            EventFactory.GenerateEventNameDelegate generateEventName = eventType => GenerateEventName(eventType, eventIdGenerator);

            var buildEvent = EventFactory.CreateFactory(getEventTypeName, generateEventName);
            var buildNode = NodeFactory.CreateFactory(buildEvent);
            var buildRoute = RouteFactory.CreateFactory(buildNode, getRouteName, GenerateNodeName);
            var buildRouteSet = RouteSetFactory.CreateFactory(buildRoute);

            var routeSetGameObject = buildRouteSet.Invoke(routeSet, Path.GetFileNameWithoutExtension(ctx.assetPath));

            ctx.AddObjectToAsset(routeSetGameObject.name, routeSetGameObject.gameObject);
            ctx.SetMainObject(routeSetGameObject.gameObject);
        }

        private static string GenerateNodeName(string routeName, int nodeIndex)
        {
            return String.Format("{0}_Node{1:0000}", routeName, nodeIndex.ToString("0000"));
        }

        private static string GenerateEventName(string eventType, EventIdGenerator idGenerator)
        {
            return String.Format("{0}_{1:0000}", eventType, idGenerator.Generate());
        }

        private class EventIdGenerator
        {
            private int lastId;

            public int Generate()
            {
                lastId++;
                return lastId;
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