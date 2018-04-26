namespace FoxKit.Modules.FormVariation.Importer
{
    using System;
    using System.IO;

    using UnityEditor.Experimental.AssetImporters;

    using FoxKit.Modules.FormVariation;

    /// <summary>
    /// ScriptedImporter to handle importing fv2 files.
    /// </summary>
    [ScriptedImporter(1, "fv2")]
    public class FormVariationImporter : ScriptedImporter
    {
        /// <summary>
        /// Import a .fv2 file.
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            FoxLib.FormVariation.FormVariation formVariation = null;

            using (var reader = new BinaryReader(new FileStream(ctx.assetPath, FileMode.Open)))
            {
                Action<int> skipBytes = numberOfBytes => SkipBytes(reader, numberOfBytes);
                Action<long> moveStream = bytePos => MoveStream(reader, bytePos);
                var readFunctions = new FoxLib.FormVariation.ReadFunctions(reader.ReadUInt16, reader.ReadUInt32, reader.ReadUInt64, reader.ReadByte, skipBytes, moveStream);
                formVariation = FoxLib.FormVariation.Read(readFunctions);
            }

            var formVariationSet = UnityEngine.ScriptableObject.CreateInstance<FormVariation>();

            var fova = FormVariation.makeFormVariation(formVariation);

            ctx.AddObjectToAsset("fv2", fova);
            ctx.SetMainObject(fova);
        }

        /// <summary>
        /// Skip reading a number of bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="numberOfBytes">The number of bytes to skip.</param>
        private static void SkipBytes(BinaryReader reader, long numberOfBytes)
        {
            reader.BaseStream.Position += numberOfBytes;
        }

        /// <summary>
        /// Move stream to a given position in bytes.
        /// </summary>
        /// <param name="reader">The BinaryReader to use.</param>
        /// <param name="bytePos">The byte number to move the stream position to.</param>
        private static void MoveStream(BinaryReader reader, long bytePos)
        {
            reader.BaseStream.Position = bytePos;
        }
    }
}