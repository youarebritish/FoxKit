namespace FoxKit.Modules.PartsBuilder.FormVariation.Exporter
{
    using System.IO;

    using UnityEngine.Assertions;

    public static class FormVariationExporter
    {
        /// <summary>
        /// Exports FormVariation to an fv2 file.
        /// </summary>
        /// <param name="foxKitFormVariation">The Form Variation to export.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportFormVariation(FormVariation foxKitFormVariation, string exportPath)
        {
            Assert.IsNotNull(exportPath, "exportPath must not be null.");

            FoxLib.FormVariation.FormVariation foxLibFormVariation = foxKitFormVariation.Convert();

            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                System.Action<int> writeEmptyBytes = numberOfBytes => WriteEmptyBytes(writer, numberOfBytes);
                System.Func<long> getWriterPosition = () => writer.BaseStream.Position;
                var writeFunctions = new FoxLib.FormVariation.WriteFunctions(writer.Write, writer.Write, writer.Write, writer.Write, writer.Write, writer.Write, writeEmptyBytes, getWriterPosition);
                FoxLib.FormVariation.Write(foxLibFormVariation, writeFunctions);
            }
        }

        /// <summary>
        /// Writes a number of empty bytes.
        /// </summary>
        /// <param name="writer">The BinaryWriter to use.</param>
        /// <param name="numberOfBytes">The number of empty bytes to write.</param>
        private static void WriteEmptyBytes(BinaryWriter writer, int numberOfBytes)
        {
            writer.Write(new byte[numberOfBytes]);
        }
    }
}