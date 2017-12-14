namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using GzsTool.Core.Common;

    using UnityEngine.Assertions;

    /// <summary>
    /// Sorts archive files by their file extension.
    /// </summary>
    public class FileRegistry
    {
        /// <summary>
        /// Maps extension (without a dot) to files with that extension.
        /// </summary>
        private readonly Dictionary<string, HashSet<FileDataStreamContainer>> fileMap = new Dictionary<string, HashSet<FileDataStreamContainer>>();

        /// <summary>
        /// List of supported file extensions.
        /// </summary>
        private readonly HashSet<string> supportedExtensions = new HashSet<string>();

        /// <summary>
        /// Delegate for when a file is registered.
        /// </summary>
        /// <param name="file">File that was registered.</param>
        /// <param name="extension">Extension of the file.</param>
        public delegate void OnFileRegisteredDelegate(FileDataStreamContainer file, string extension);

        /// <summary>
        /// Event raised when a file is registered.
        /// </summary>
        public event OnFileRegisteredDelegate OnFileRegistered;

        /// <summary>
        /// Gets total number of registered files.
        /// </summary>
        public int FileCount { get; private set; }

        /// <summary>
        /// Gets the file extension (without leading dot) for a filename.
        /// </summary>
        /// <param name="filename">Filename, including extension.</param>
        /// <returns>The file extension (without leading dot).</returns>
        public static string GetExtension(string filename)
        {
            return Regex.Match(filename, @"\..*").Value.Remove(0, 1);
        }

        /// <summary>
        /// Registers a unique file.
        /// </summary>
        /// <param name="file">The file to register.</param>
        public void RegisterFile(FileDataStreamContainer file)
        {
            Assert.IsNotNull(file, "Input file must not be null.");

            var extension = GetExtension(file.FileName);

            if (HasExtensionAlreadyBeenRegistered(extension, this.fileMap))
            {
                var extensionEntry = this.fileMap[extension];
                extensionEntry.Add(file);
            }
            else
            {
                var contents = new HashSet<FileDataStreamContainer> { file };
                this.fileMap.Add(extension, contents);
            }

            this.FileCount++;

            this.OnFileRegistered?.Invoke(file, extension);
        }

        /// <summary>
        /// Determines whether or not a file has already been registered.
        /// </summary>
        /// <param name="file">File to check.</param>
        /// <returns>True if the file is already present in the map, otherwise false.</returns>
        public bool ContainsFile(FileDataStreamContainer file)
        {
            Assert.IsNotNull(file, "Input file must not be null.");

            var extension = GetExtension(file.FileName);
            return HasFileAlreadyBeenRegistered(file, extension, this.fileMap);
        }

        /// <summary>
        /// Determines whether or not a file extension has already been registered.
        /// </summary>
        /// <param name="extension">Extension to check.</param>
        /// <returns>True if the file extension is already present in the map, otherwise false.</returns>
        public bool ContainsExtension(string extension)
        {
            return this.fileMap.ContainsKey(extension);
        }

        /// <summary>
        /// Determines whether or not a file extension is supported.
        /// </summary>
        /// <param name="extension">Extension to check.</param>
        /// <returns>True if the file extension is supported, else false.</returns>
        public bool SupportsExtension(string extension)
        {
            return this.supportedExtensions.Contains(extension);
        }

        /// <summary>
        /// Gets the set of registered files with an extension.
        /// </summary>
        /// <param name="extension">Extension (without leading dot) to query.</param>
        /// <returns>The set of registered files with the given extension.</returns>
        public HashSet<FileDataStreamContainer> GetFilesWithExtension(string extension)
        {
            return this.fileMap[extension];
        }

        /// <summary>
        /// Add a file extension that's supported for extraction.
        /// </summary>
        /// <param name="extension">
        /// The extension (without a leading dot).
        /// </param>
        public void AddSupportedExtension(string extension)
        {
            this.supportedExtensions.Add(extension);
        }

        /// <summary>
        /// Print the registered extensions and number of files with each extension.
        /// </summary>
        public void PrintStats()
        {
            foreach (var extension in this.fileMap.Keys)
            {
                UnityEngine.Debug.Log(extension + ": " + this.fileMap[extension].Count + " files registered.");
            }
        }
        
        /// <summary>
        /// Determines whether or not a file extension has already been registered.
        /// </summary>
        /// <param name="extension">Extension (without leading dot) to check.</param>
        /// <param name="map">The file map.</param>
        /// <returns>True if the file extension is already present in the map, otherwise false.</returns>
        private static bool HasExtensionAlreadyBeenRegistered(string extension, Dictionary<string, HashSet<FileDataStreamContainer>> map)
        {
            return map.ContainsKey(extension);
        }

        /// <summary>
        /// Determines whether or not a file has already been registered.
        /// </summary>
        /// <param name="file">File to check.</param>
        /// <param name="extension">Extension (without leading dot) of the file.</param>
        /// <param name="map">The file map.</param>
        /// <returns>True if the file is already present in the map, otherwise false.</returns>
        private static bool HasFileAlreadyBeenRegistered(FileDataStreamContainer file, string extension, Dictionary<string, HashSet<FileDataStreamContainer>> map)
        {
            return HasExtensionAlreadyBeenRegistered(extension, map) && map[extension].Contains(file);
        }
    }
}