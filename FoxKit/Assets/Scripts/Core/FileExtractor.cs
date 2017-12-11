using FoxKit.Modules.FormatHandlers.ArchiveHandler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Assertions;
using static FoxKit.Modules.FormatHandlers.ArchiveHandler.ArchiveHandler;

namespace FoxKit.Core
{
    /// <summary>
    /// Extracts files to a useful format.
    /// </summary>
    public class FileExtractor
    {
        /// <summary>
        /// Base directory to write files to.
        /// </summary>
        private readonly string OutputDirectory;

        /// <summary>
        /// Prioritized list of file format converters. They will be run in the order they're provided.
        /// </summary>
        private readonly List<IFormatHandler> FormatHandlers;

        private readonly BeginExtractingFileDelegate OnBeginExtractingFile;
        private readonly IncrementExtractedFileCountDelegate IncrementExtractedFileCount;
        private readonly GetExtractingArchiveFilenameDelegate GetExtractingArchiveFilename;

        public FileExtractor(string outputDirectory, List<IFormatHandler> formatHandlers, BeginExtractingFileDelegate onBeginExtractingFile, IncrementExtractedFileCountDelegate incrementExtractedFileCount, GetExtractingArchiveFilenameDelegate getExtractingArchiveFilename)
        {
            Assert.IsNotNull(outputDirectory, "outputDirectory must not be null.");
            Assert.IsNotNull(formatHandlers, "fileRegistry must not be null.");

            Assert.IsNotNull(onBeginExtractingFile, "onBeginExtractingFile must not be null.");
            Assert.IsNotNull(incrementExtractedFileCount, "incrementExtractedFileCount must not be null.");
            Assert.IsNotNull(getExtractingArchiveFilename, "getExtractingArchiveFilename must not be null.");

            OutputDirectory = outputDirectory;
            FormatHandlers = formatHandlers;

            OnBeginExtractingFile = onBeginExtractingFile;
            IncrementExtractedFileCount = incrementExtractedFileCount;
            GetExtractingArchiveFilename = getExtractingArchiveFilename;
        }

        /// <summary>
        /// Extracts the files in a FileRegistry.
        /// </summary>
        /// <param name="fileRegistry">Registry of files to extract.</param>
        public void ExtractFiles(FileRegistry fileRegistry)
        {
            Assert.IsNotNull(fileRegistry, "fileRegistry must not be null.");

            var extractedFiles = new Dictionary<string, object>();

            foreach (var formatHandler in FormatHandlers)
            {
                foreach (var extension in formatHandler.Extensions)
                {
                    if (!fileRegistry.ContainsExtension(extension))
                    {
                        continue;
                    }

                    foreach (var file in fileRegistry.GetFilesWithExtension(extension))
                    {
                        if (extractedFiles.ContainsKey(file.FileName))
                        {
                            continue;
                        }

                        OnBeginExtractingFile.Invoke(file.FileName, fileRegistry.FileCount, IncrementExtractedFileCount, GetExtractingArchiveFilename);

                        var outputFilePath = MakeOutputPath(OutputDirectory, file.FileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                        extractedFiles.Add(file.FileName, formatHandler.Import(file.DataStream(), outputFilePath));
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a specific file. Can be used to extract a file required by another one that's already being extracted.
        /// </summary>
        /// <param name="input">Input stream.</param>
        /// <param name="filename">Filename of the file to extract.</param>
        /// <param name="extension">Extension of the file to extract.</param>
        /// <param name="outputDirectory">Output directory to write the file.</param>
        /// <param name="fileRegistry">Registry of all discovered files.</param>
        /// <param name="formatHandlers">Set of format handlers to use.</param>
        /// <returns>The extracted file.</returns>
        private static object ExtractFile(Stream input, string filename, string extension, string outputDirectory, FileRegistry fileRegistry, List<IFormatHandler> formatHandlers, Dictionary<string, object> extractedFiles)
        {
            Assert.IsNotNull(input, "Input stream must not be null.");
            Assert.IsNotNull(filename, "Input filename must not be null.");
            Assert.IsNotNull(extension, "Input extension must not be null.");
            Assert.IsNotNull(outputDirectory, "outputDirectory must not be null.");
            Assert.IsNotNull(fileRegistry, "fileRegistry must not be null.");
            Assert.IsNotNull(formatHandlers, "formatHandlers must not be null.");
            Assert.IsNotNull(extractedFiles, "extractedFiles must not be null.");

            if (extractedFiles.ContainsKey(filename))
            {
                return extractedFiles[filename];
            }

            var formatHandler = FindFormatHandlerForExtension(extension, formatHandlers);
            var extractedFile = formatHandler.Import(input, MakeOutputPath(outputDirectory, filename));

            extractedFiles.Add(filename, extractedFile);
            return extractedFile;
        }

        /// <summary>
        /// Find a relevant format handler for the given file extension.
        /// </summary>
        /// <param name="extension">Extension to find a format handler for.</param>
        /// <param name="formatHandlers">Set of format handlers to search through.</param>
        /// <returns>A format handler that can handle the file extension, or null if none was found.</returns>
        private static IFormatHandler FindFormatHandlerForExtension(string extension, List<IFormatHandler> formatHandlers)
        {
            return formatHandlers.First(handler => handler.Extensions.Contains(extension));
        }

        /// <summary>
        /// Makes the output path for a file.
        /// </summary>
        /// <param name="outputDirectory">Base output directory.</param>
        /// <param name="filename">Filename of the file.</param>
        /// <returns>The file's output path.</returns>
        private static string MakeOutputPath(string outputDirectory, string filename)
        {
            return Path.Combine(outputDirectory, filename);
        }
    }
}