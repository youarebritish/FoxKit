using System.IO;

namespace FoxKit.Modules.RailBuilder.Exporter
{
    /// <summary>
    /// Collection of helper functions for exporting RailUniqueIdSets to frld format.
    /// </summary>
    public static class RailUniqueIdSetExporter
    {
        /// <summary>
        /// Exports RailUniqueIdSet to a frld file.
        /// </summary>
        /// <param name="railIds">Rail IDs to export.</param>
        /// <param name="exportPath">File path to export to.</param>
        public static void ExportRailUniqueIdSet(uint[] railIds, string exportPath)
        {
            using (var writer = new BinaryWriter(new FileStream(exportPath, FileMode.Create)))
            {
                var writeFunctions = new FoxLib.Tpp.RailUniqueIdFile.WriteFunctions(writer.Write, writer.Write, writer.Write);
                FoxLib.Tpp.RailUniqueIdFile.Write(writeFunctions, railIds);
            }
        }
    }
}