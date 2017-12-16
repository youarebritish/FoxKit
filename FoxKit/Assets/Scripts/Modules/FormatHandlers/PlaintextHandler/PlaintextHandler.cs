namespace FoxKit.Modules.FormatHandlers.PlaintextHandler
{
    using System.Collections.Generic;
    using System.IO;

    using FoxKit.Core;

    /// <inheritdoc />
    /// <summary>
    /// Imports and exports plaintext files.
    /// </summary>
    public class PlaintextHandler : IFormatHandler
    {
        /// <inheritdoc />
        public List<string> Extensions { get; } = new List<string> { "lua", "json", "fpkl", "dat" };

        /// <inheritdoc />
        public object Import(Stream input, string path)
        {
            // TODO: Support changing the file extension (fpkl -> json, dat -> xml)
            using (var outputStream = new FileStream(path, FileMode.Create))
            {
                input.CopyTo(outputStream);
                return outputStream;
            }
        }

        /// <inheritdoc />
        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }
    }
}