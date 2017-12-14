namespace FoxKit.Core
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// A generic interface for classes that can import/export Fox Engine file formats.
    /// </summary>
    public interface IFormatHandler
    {
        /// <summary>
        /// Gets the extension(s) this handler can import/export.
        /// </summary>
        /// <example>"lua" (no period)</example>
        List<string> Extensions { get; }

        /// <summary>
        /// Imports a Fox Engine asset from a binary stream.
        /// </summary>
        /// <param name="input">Binary stream of a Fox asset.</param>
        /// <param name="path">Filename of the importing asset.</param>
        /// <returns>The converted asset.</returns>
        object Import(Stream input, string path);

        /// <summary>
        /// Export a Unity asset to Fox Engine format.
        /// </summary>
        /// <param name="asset">Object to export to Fox Engine format.</param>
        /// <param name="path">Path to output the resulting file to.</param>
        void Export(object asset, string path);
    }
}