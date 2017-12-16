namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    using System.Collections.Generic;
    using System.IO;

    using FoxKit.Core;

    using GzsTool.Core.Common;
    using GzsTool.Core.Fpk;
    using GzsTool.Core.Pftxs;
    using GzsTool.Core.Qar;
    using GzsTool.Core.Sbp;

    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Imports and exports archives.
    /// </summary>
    public class ArchiveHandler : IFormatHandler
    {
        /// <summary>
        /// Table of discovered files.
        /// </summary>
        private readonly FileRegistry fileRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveHandler"/> class.
        /// </summary>
        /// <param name="fileRegistry">
        /// Table to store discovered files.
        /// </param>
        public ArchiveHandler(FileRegistry fileRegistry)
        {
            this.fileRegistry = fileRegistry;
        }
        
        /// <summary>
        /// Delegate for when a file begins being extracted.
        /// </summary>
        /// <param name="filename">Filename of the file being extracted.</param>
        /// <param name="totalFileCount">Total number of files in the archive to extract.</param>
        /// <param name="incrementExtractedFileCountDelegate">Delegate to increment the total number of already extracted files.</param>
        /// <param name="getExtractingArchiveFilenameDelegate">Delegate to get the filename of the archive currently being extracted.</param>
        public delegate void BeginExtractingFileDelegate(string filename, int totalFileCount, IncrementExtractedFileCountDelegate incrementExtractedFileCountDelegate, GetExtractingArchiveFilenameDelegate getExtractingArchiveFilenameDelegate);
        
        /// <summary>
        /// Delegate for incrementing the number of extracted files.
        /// </summary>
        /// <returns>The new number of extracted files.</returns>
        public delegate int IncrementExtractedFileCountDelegate();

        /// <summary>
        /// Delegate for resetting the number of extracted files.
        /// </summary>
        public delegate void ResetExtractedFileCountDelegate();

        /// <summary>
        /// Delegate for getting the filename of the archive currently being extracted.
        /// </summary>
        /// <returns>Filename of the archive currently being extracted.</returns>
        public delegate string GetExtractingArchiveFilenameDelegate();

        /// <summary>
        /// Delegate for setting the filename of the archive currently being read.
        /// </summary>
        /// <param name="archiveFilename">
        /// The archive filename.
        /// </param>
        public delegate void SetReadingArchiveFilenameDelegate(string archiveFilename);

        /// <inheritdoc />
        public List<string> Extensions { get; } = new List<string> { "fpk" };//{ "pftxs", "fpk", "fpkd", "sbp" };

        /// <inheritdoc />
        public object Import(Stream input, string path)
        {
            //UnityEngine.Debug.Log("Extracting " + path);
            Assert.IsNotNull(input, "input stream must not be null.");
            Assert.IsNotNull(path, "input path must not be null.");

            var filename = Path.GetFileName(path);
            var extension = FileRegistry.GetExtension(path);
            var archiveFile = ReadArchive(filename, extension, input);
            var exportFiles = archiveFile.ExportFiles(input);

            foreach (var exportedFile in exportFiles)
            {
                // Don't extract a file more than once, even if there are duplicates of it.
                if (this.fileRegistry.ContainsFile(exportedFile))
                {
                    continue;
                }

                // Don't add files whose extension we can't do anything with.
                if (!this.fileRegistry.SupportsExtension(FileRegistry.GetExtension(exportedFile.FileName)))
                {
                    continue;
                }
                this.fileRegistry.RegisterFile(exportedFile);
            }

            return exportFiles;
        }

        /// <inheritdoc />
        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Reads an archive.
        /// </summary>
        /// <param name="filename">
        /// The filename of the archive.
        /// </param>
        /// <param name="extension">
        /// The extension of the archive.
        /// </param>
        /// <param name="inputStream">
        /// The input stream.
        /// </param>
        /// <returns>
        /// The <see cref="ArchiveFile"/>.
        /// </returns>
        private static ArchiveFile ReadArchive(string filename, string extension, Stream inputStream)
        {
            ArchiveFile file = null;
            switch (extension)
            {
                case "fpk":
                case "fpkd":
                    file = new FpkFile();
                    break;
                case "dat":
                    file = new QarFile();
                    break;
                case "pftxs":
                    file = new PftxsFile();
                    break;
                case "sbp":
                    file = new SbpFile();
                    break;
                default:
                    Assert.IsTrue(false, "Invalid archive extension: " + extension);
                    break;
            }

            file.Name = filename;
            file.Read(inputStream);
            return file;
        }
    }
}