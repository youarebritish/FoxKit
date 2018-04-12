namespace FoxKit.Modules.RouteBuilder.Importer
{
    using System.IO;

    using FoxKit.Core;

    using UnityEditor.Experimental.AssetImporters;
    using System;
    using FoxKit.Modules.RouteBuilder;

    /// <summary>
    /// ScriptedImporter to handle importing frt files.
    /// </summary>
    [ScriptedImporter(1, "frt")]
    public class RouteSetImporter : ScriptedImporter
    {
        /// <summary>
        /// Hash manager for Route names.
        /// </summary>
        private IHashManager<uint> routeNameHashManager;

        /// <summary>
        /// Hash manager for RouteEvent types.
        /// </summary>
        private IHashManager<uint> eventTypeHashManager;

        /// <summary>
        /// Hash manager for RouteEvent messages.
        /// </summary>
        private IHashManager<uint> messageHashManager;
        
        /// <summary>
        /// Import a .frt file.
        /// </summary>
        /// <param name="ctx"></param>
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
            var getEventTypeName = eventTypeHashManager.MakeUnhashFunc();

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

        /// <summary>
        /// Generates a route node name.
        /// </summary>
        /// <param name="routeName">Name of the owning route.</param>
        /// <param name="nodeIndex">Index of the node.</param>
        /// <returns>A new route node name.</returns>
        private static string GenerateNodeName(string routeName, int nodeIndex)
        {
            return String.Format("{0}_Node{1:0000}", routeName, nodeIndex.ToString("0000"));
        }

        /// <summary>
        /// Generates a route event name.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="idGenerator">ID number generator.</param>
        /// <returns>A new route event name.</returns>
        private static string GenerateEventName(string eventType, EventIdGenerator idGenerator)
        {
            return String.Format("{0}_{1:0000}", eventType, idGenerator.Generate());
        }

        /// <summary>
        /// Generates route event IDs.
        /// </summary>
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
                
        /// <summary>
        /// Initialize hash dictionaries.
        /// </summary>
        private void InitializeDictionaries()
        {
            this.routeNameHashManager = new StrCode32HashManager();
            this.eventTypeHashManager = new StrCode32HashManager();
            this.messageHashManager = new StrCode32HashManager();

            this.routeNameHashManager.LoadDictionary(RouteBuilderPreferences.Instance.IdDictionary);
            this.eventTypeHashManager.LoadDictionary(RouteBuilderPreferences.Instance.EventDictionary);
            this.messageHashManager.LoadDictionary(RouteBuilderPreferences.Instance.MessageDictionary);
        }
    }
}