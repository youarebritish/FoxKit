using System.Collections.Generic;
using System.IO;
using FoxKit.Core;
using GzsTool.Core.Qar;
using GzsTool.Core.Common.Interfaces;
using GzsTool.Core.Common;
using System.Linq;
using UnityEngine.Assertions;
using System;
using UnityEngine;

namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    /// <summary>
    /// Imports and exports QAR archives.
    /// </summary>
    public class ArchiveHandler : IFormatHandler
    {
        #region Fields
        public List<string> Extensions => SupportedExtensions;

        private readonly List<string> SupportedExtensions = new List<string>() { "dat", "g0s" };        
        private readonly FileRegistry FileRegistry;
        #endregion

        public ArchiveHandler(FileRegistry fileRegistry)
        {
            FileRegistry = fileRegistry;           
        }

        #region Delegates
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
        /// Delegate for setting the filename of the archive currently being extracted.
        /// </summary>
        /// <returns>Filename of the archive currently being extracted.</returns>
        public delegate void SetExtractingArchiveFilenameDelegate(string archiveFilename);
        #endregion

        public object Import(Stream input, string path)
        {
            Assert.IsNotNull(input, "input stream must not be null.");
            Assert.IsNotNull(path, "input path must not be null.");
            
            var archiveFile = ReadArchive(path, input);
            var exportFiles = archiveFile.ExportFiles(input);

            foreach (var exportedFile in exportFiles)
            {
                // Don't extract a file more than once, even if there are duplicates of it.
                if (FileRegistry.ContainsFile(exportedFile))
                {
                    continue;
                }
                // Don't add files whose extension we can't do anything with.
                if (!FileRegistry.SupportsExtension(FileRegistry.GetExtension(exportedFile.FileName)))
                {
                    continue;
                }
                FileRegistry.RegisterFile(exportedFile);
            }

            return exportFiles;
        }

        public void Export(object asset, string path)
        {
            throw new System.NotImplementedException();
        }

        private static ArchiveFile ReadArchive(string path, Stream inputStream)
        {
            var file = new QarFile();
            file.Name = Path.GetFileName(path);
            file.Read(inputStream);
            return file;
        }
    }
}