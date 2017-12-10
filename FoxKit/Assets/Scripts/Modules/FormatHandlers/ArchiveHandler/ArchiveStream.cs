using System.IO;

namespace FoxKit.Modules.FormatHandlers.ArchiveHandler.Editor
{
    public class ArchiveStream
    {
        public string Path
        {
            get
            {
                return ArchivePath;
            }
        }

        private readonly Stream InputStream;
        private readonly string ArchivePath;

        public ArchiveStream(Stream inputStream, string archivePath)
        {
            InputStream = inputStream;
            ArchivePath = archivePath;
        }

        public void Read(ArchiveHandler archiveHandler)
        {
            archiveHandler.Import(InputStream, ArchivePath);
        }

        public void Close()
        {
            InputStream.Close();
        }
    }
}