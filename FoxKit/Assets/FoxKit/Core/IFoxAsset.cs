namespace FoxKit.Core
{
    using System.IO;

    /// <summary>
    /// Interface for assets which can be exported to a Fox Engine format.
    /// </summary>
    public interface IFoxAsset
    {
        /// <summary>
        /// Exports the asset.
        /// </summary>
        /// <param name="outputStream">The stream to write to.</param>
        void Export(Stream outputStream);
    }
}