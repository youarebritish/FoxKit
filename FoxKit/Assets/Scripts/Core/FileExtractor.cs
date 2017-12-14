namespace FoxKit.Core
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.FormatHandlers.ArchiveHandler;

    using UnityEngine.Assertions;

    /// <summary>
    /// Extracts files to a useful format.
    /// </summary>
    public class FileExtractor
    {
        /// <summary>
        /// Base directory to write files to.
        /// </summary>
        private readonly string outputDirectory;

        /// <summary>
        /// Prioritized list of file format converters. They will be run in the order they're provided.
        /// </summary>
        private readonly List<IFormatHandler> formatHandlers;

        /// <summary>
        /// Invoked when a file begins being extracted.
        /// </summary>
        private readonly ArchiveHandler.BeginExtractingFileDelegate onBeginExtractingFile;

        /// <summary>
        /// Delegate to increment the count of extracted files by one.
        /// </summary>
        private readonly ArchiveHandler.IncrementExtractedFileCountDelegate incrementExtractedFileCount;

        /// <summary>
        /// Delegate to get the filename of the archive currently being extracted.
        /// </summary>
        private readonly ArchiveHandler.GetExtractingArchiveFilenameDelegate getExtractingArchiveFilename;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtractor"/> class.
        /// </summary>
        /// <param name="outputDirectory">
        /// The output directory to extract to.
        /// </param>
        /// <param name="formatHandlers">
        /// Format handlers to use for converting file types.
        /// </param>
        /// <param name="onBeginExtractingFile">
        /// Callback to invoke when a file begins being extracted.
        /// </param>
        /// <param name="incrementExtractedFileCount">
        /// Callback to increment the extracted file count by one.
        /// </param>
        /// <param name="getExtractingArchiveFilename">
        /// Delegate to get the filename of the archive currently being extracted.
        /// </param>
        public FileExtractor(string outputDirectory, List<IFormatHandler> formatHandlers, ArchiveHandler.BeginExtractingFileDelegate onBeginExtractingFile, ArchiveHandler.IncrementExtractedFileCountDelegate incrementExtractedFileCount, ArchiveHandler.GetExtractingArchiveFilenameDelegate getExtractingArchiveFilename)
        {
            Assert.IsNotNull(outputDirectory, "outputDirectory must not be null.");
            Assert.IsNotNull(formatHandlers, "fileRegistry must not be null.");

            Assert.IsNotNull(onBeginExtractingFile, "onBeginExtractingFile must not be null.");
            Assert.IsNotNull(incrementExtractedFileCount, "incrementExtractedFileCount must not be null.");
            Assert.IsNotNull(getExtractingArchiveFilename, "getExtractingArchiveFilename must not be null.");

            this.outputDirectory = outputDirectory;
            this.formatHandlers = formatHandlers;

            this.onBeginExtractingFile = onBeginExtractingFile;
            this.incrementExtractedFileCount = incrementExtractedFileCount;
            this.getExtractingArchiveFilename = getExtractingArchiveFilename;
        }

        /// <summary>
        /// Extracts the files in a FileRegistry.
        /// </summary>
        /// <param name="fileRegistry">Registry of files to extract.</param>
        public void ExtractFiles(FileRegistry fileRegistry)
        {
            Assert.IsNotNull(fileRegistry, "fileRegistry must not be null.");
            
            var extractedFiles = new Dictionary<string, object>();

            var onFileRegisteredWhileExtracting = MakeOnFileRegisteredWhileExtractingDelegate(this.outputDirectory, this.formatHandlers, extractedFiles);
            fileRegistry.OnFileRegistered += onFileRegisteredWhileExtracting;

            foreach (var formatHandler in this.formatHandlers)
            {
                foreach (var extension in formatHandler.Extensions)
                {
                    if (!fileRegistry.ContainsExtension(extension))
                    {
                        continue;
                    }

                    foreach (var file in fileRegistry.GetFilesWithExtension(extension))
                    {
                        // TODO Should OnBeginExtractingFile go inside ExtractFile? Otherwise the progress bar UI won't be extracting when loading required files.
                        this.onBeginExtractingFile.Invoke(file.FileName, fileRegistry.FileCount, this.incrementExtractedFileCount, this.getExtractingArchiveFilename);
                        ExtractFile(file.DataStream(), file.FileName, this.outputDirectory, formatHandler, extractedFiles);
                    }
                }
            }

            fileRegistry.OnFileRegistered -= onFileRegisteredWhileExtracting;
        }

        /// <summary>
        /// Make the callback to invoke when a file is registered while another is already being extracted.
        /// </summary>
        /// <param name="outputDirectory">
        /// The output directory to write to.
        /// </param>
        /// <param name="formatHandlers">
        /// Format handlers to use for converting file types.
        /// </param>
        /// <param name="extractedFiles">
        /// Table of extracted files.
        /// </param>
        /// <returns>
        /// The delegate to invoke.
        /// </returns>
        private static FileRegistry.OnFileRegisteredDelegate MakeOnFileRegisteredWhileExtractingDelegate(string outputDirectory, List<IFormatHandler> formatHandlers, Dictionary<string, object> extractedFiles)
        {
            return (file, extension) => OnFileRegisteredWhileExtracting(
                file.DataStream(),
                file.FileName,
                extension,
                outputDirectory,
                formatHandlers,
                extractedFiles);
        }

        /// <summary>
        /// Called when a file is registered while another is already being extracted. Immediately tries to extract the newly-registered file.
        /// </summary>
        /// <param name="input">
        /// Input stream containing the contents of the newly-registered file.
        /// </param>
        /// <param name="filename">
        /// Filename of the newly-registered file.
        /// </param>
        /// <param name="extension">
        /// Extension of the newly-registered file.
        /// </param>
        /// <param name="outputDirectory">
        /// Output directory to write to.
        /// </param>
        /// <param name="formatHandlers">
        /// Format handlers to use for converting file types.
        /// </param>
        /// <param name="extractedFiles">
        /// Table of extracted files.
        /// </param>
        private static void OnFileRegisteredWhileExtracting(Stream input, string filename, string extension, string outputDirectory, List<IFormatHandler> formatHandlers, Dictionary<string, object> extractedFiles)
        {
            var formatHandler = FindFormatHandlerForExtension(extension, formatHandlers);
            Assert.IsNotNull(formatHandler, "No format handler found for extension " + extension);

            ExtractFile(input, filename, outputDirectory, formatHandler, extractedFiles);
        }

        /// <summary>
        /// Extracts a specific file. Can be used to extract a file required by another one that's already being extracted.
        /// </summary>
        /// <param name="input">
        /// Input stream containing the contents of the file.
        /// </param>
        /// <param name="filename">
        /// Filename of the file to extract.
        /// </param>
        /// <param name="outputDirectory">
        /// Output directory to write the file.
        /// </param>
        /// <param name="formatHandler">
        /// Format handler to use.
        /// </param>
        /// <param name="extractedFiles">
        /// Table of extracted files.
        /// </param>
        /// <returns>
        /// The extracted file.
        /// </returns>
        private static object ExtractFile(Stream input, string filename, string outputDirectory, IFormatHandler formatHandler, Dictionary<string, object> extractedFiles)
        {
            Assert.IsNotNull(input, "Input stream must not be null.");
            Assert.IsNotNull(filename, "Input filename must not be null.");
            Assert.IsNotNull(outputDirectory, "outputDirectory must not be null.");
            Assert.IsNotNull(formatHandler, "formatHandler must not be null.");
            Assert.IsNotNull(extractedFiles, "extractedFiles must not be null.");
            
            if (extractedFiles.ContainsKey(filename))
            {
                return extractedFiles[filename];
            }

            var outputFilePath = MakeOutputPath(outputDirectory, filename);
            
            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
            var extractedFile = formatHandler.Import(input, outputFilePath);

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