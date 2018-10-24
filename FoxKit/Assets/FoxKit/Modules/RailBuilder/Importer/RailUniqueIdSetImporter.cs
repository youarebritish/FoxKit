namespace FoxKit.Modules.RailBuilder.Importer
{
    using System.IO;
    
    using UnityEditor.Experimental.AssetImporters;
    using System;

    /// <summary>
    /// ScriptedImporter to handle importing frld files.
    /// </summary>
    [ScriptedImporter(1, "frld")]
    public class RailUniqueIdSetImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .frld file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var railIdsAsset = UnityEngine.ScriptableObject.CreateInstance<RailUniqueIdSet>();
            railIdsAsset.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            railIdsAsset.IsReadOnly = true;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open), FoxLib.Tpp.RouteSet.getEncoding()))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                var readFunctions = new FoxLib.Tpp.RailUniqueIdFile.ReadFunctions(reader.ReadUInt16, reader.ReadUInt32, skipBytes);
                railIdsAsset.Ids = FoxLib.Tpp.RailUniqueIdFile.Read(readFunctions);
            }
            
            ctx.AddObjectToAsset(railIdsAsset.name, railIdsAsset);
            ctx.SetMainObject(railIdsAsset);
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
    }
        
}