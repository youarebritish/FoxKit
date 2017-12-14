namespace FoxKit.Modules.FormatHandlers.ArchiveHandler.Editor
{
    using System.IO;

    /// <summary>
    /// Encapsulates a stream for reading an archive.
    /// </summary>
    public class ArchiveStream
    {
        /// <summary>
        /// The input stream.
        /// </summary>
        private readonly Stream inputStream;

        /// <summary>
        /// Gets the path to the archive.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveStream"/> class.
        /// </summary>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <param name="archivePath">
        /// The archive path.
        /// </param>
        public ArchiveStream(Stream inputStream, string archivePath)
        {
            this.inputStream = inputStream;
            this.Path = archivePath;
        }

        /// <summary>
        /// Read the archive.
        /// </summary>
        /// <param name="archiveHandler">
        /// Handler for processing the archive format.
        /// </param>
        public void Read(ArchiveHandler archiveHandler)
        {
            archiveHandler.Import(this.inputStream, this.Path);
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        public void Close()
        {
            this.inputStream.Close();
        }
    }
}