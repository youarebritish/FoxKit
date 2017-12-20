namespace FoxKit.Modules.FormatHandlers.ArchiveHandler.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Core;
    using FoxKit.Modules.FormatHandlers.PlaintextHandler;
    using FoxKit.Modules.FormatHandlers.TextureHandler;

    using GzsTool.Core.Utility;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Custom editor for GameProfiles.
    /// </summary>
    [CustomEditor(typeof(GameProfile))]
    public class GameProfileEditor : Editor
    {
        #region Constants
        #region UI strings
        /// <summary>
        /// Label text for the unpack game button.
        /// </summary>
        private const string UnpackButtonLabel = "Unpack Game";

        /// <summary>
        /// Text to display in the title of the open folder dialog.
        /// </summary>
        private const string SelectGameDirectoryPanelTitle = "Select game directory";

        /// <summary>
        /// Text to display in the title of the progress bar while reading an archive.
        /// </summary>
        private const string ReadingArchiveProgressBarTitle = "Reading {0}...";

        /// <summary>
        /// Text to display in the title of the progress bar while extracting files.
        /// </summary>
        private const string ExtractingFilesProgressBarTitle = "Extracting files...";

        /// <summary>
        /// Text to display in the text of the progress bar while extracting a file.
        /// </summary>
        private const string UnpackingFileText = "Extracting {0}...";

        /// <summary>
        /// Text to display in the text of the progress bar while reading QAR entries.
        /// </summary>
        private const string ReadingEntriesText = "Reading QAR entries...";
        #endregion

        #region UI configuration
        /// <summary>
        /// Amount of spacing (in pixels) beneath the unpack game button.
        /// </summary>
        private const float UnpackButtonSpacing = 10;
        #endregion
        #endregion

        #region Fields
        /// <summary>
        /// Number of files that have already been extracted.
        /// </summary>
        private int extractedFileCount;

        /// <summary>
        /// Filename of the archive currently being extracted.
        /// </summary>
        private string readingArchiveFilename;
        #endregion

        #region Delegates
        /// <summary>
        /// Delegate for a callback invoked when an archive begins being read.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive beginning to be read.</param>
        /// <param name="setReadingArchiveFilenameDelegate">Delegate to set the filename of the archive being read.</param>
        private delegate void BeginReadingArchiveDelegate(string archiveFilename, ArchiveHandler.SetReadingArchiveFilenameDelegate setReadingArchiveFilenameDelegate);
        #endregion

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button(UnpackButtonLabel))
            {
                this.UnpackGame(target as GameProfile);
            }
            GUILayout.Space(UnpackButtonSpacing);            

            // Display the inspector fields.
            base.OnInspectorGUI();
        }
        
        /// <summary>
        /// Present the user with an "open folder" dialog to select the main directory of the game to unpack.
        /// </summary>
        /// <returns>The (absolute) path of the directory the user selected, or empty if none.</returns>
        private static string SelectGameDirectory()
        {
            return EditorUtility.OpenFolderPanel(SelectGameDirectoryPanelTitle, string.Empty, string.Empty);
        }
        
        /// <summary>
        /// Make the directory name into which to extract files.
        /// </summary>
        /// <param name="profileName">Name of the profile for the game being extracted.</param>
        /// <returns>Directory into which to extract files.</returns>
        private static string MakeExtractedFilesDirectory(string profileName)
        {
            return Application.dataPath + "/" + profileName + "/";
        }

        /// <summary>
        /// Read the QAR dictionaries so that filenames can be un-hashed.
        /// </summary>
        /// <param name="qarDictionaries">
        /// QAR dictionaries to load.
        /// </param>
        private static void ReadQarDictionaries(ICollection<TextAsset> qarDictionaries)
        {
            Assert.IsNotNull(qarDictionaries, "qarDictionaries must not be null.");

            foreach (var dictionary in qarDictionaries)
            {
                Hashing.ReadDictionary(dictionary);
            }
        }

        /// <summary>
        /// Read the FPK dictionaries so that FPK filenames can be un-hashed.
        /// </summary>
        /// <param name="fpkDictionaries">
        /// FPK dictionaries to load.
        /// </param>
        private static void ReadFpkDictionaries(ICollection<TextAsset> fpkDictionaries)
        {
            Assert.IsNotNull(fpkDictionaries, "fpkDictionaries must not be null.");

            foreach (var dictionary in fpkDictionaries)
            {
                Hashing.ReadMd5Dictionary(dictionary);
            }
        }

        /// <summary>
        /// Unpack game archives.
        /// </summary>
        /// <param name="archiveStreams">
        /// Archive streams to unpack.
        /// </param>
        /// <param name="archiveHandler">
        /// Format handler for archives.
        /// </param>
        /// <param name="onBeginReadingArchive">
        /// Callback to invoke when an archive begins being extracted.
        /// </param>
        /// <param name="setReadingArchiveFilenameDelegate">
        /// Delegate to set the name of the extracting archive.
        /// </param>
        private static void UnpackArchives(ICollection<ArchiveStream> archiveStreams, ArchiveHandler archiveHandler, BeginReadingArchiveDelegate onBeginReadingArchive, ArchiveHandler.SetReadingArchiveFilenameDelegate setReadingArchiveFilenameDelegate)
        {
            Assert.IsNotNull(archiveStreams, "archiveStreams must not be null.");
            Assert.IsNotNull(archiveHandler, "archiveHandler must not be null.");
            Assert.IsNotNull(onBeginReadingArchive, "onBeginReadingArchive must not be null.");
            Assert.IsNotNull(setReadingArchiveFilenameDelegate, "setReadingArchiveFilenameDelegate must not be null.");

            foreach (var archiveStream in archiveStreams)
            {
                onBeginReadingArchive.Invoke(archiveStream.Path, setReadingArchiveFilenameDelegate);
                UnpackArchive(archiveStream, archiveHandler);
            }
        }

        /// <summary>
        /// Unpack an archive.
        /// </summary>
        /// <param name="archiveStream">
        /// The archive stream to unpack.
        /// </param>
        /// <param name="archiveHandler">
        /// Format handler for archives.
        /// </param>
        private static void UnpackArchive(ArchiveStream archiveStream, ArchiveHandler archiveHandler)
        {
            archiveStream.Read(archiveHandler);
        }
        
        /// <summary>
        /// Called when an archive begins being unpacked.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive.</param>
        /// <param name="setReadingArchiveFilenameDelegate">Delegate to set the filename of the archive being read.</param>
        private static void OnBeginReadingArchive(string archiveFilename, ArchiveHandler.SetReadingArchiveFilenameDelegate setReadingArchiveFilenameDelegate)
        {
            setReadingArchiveFilenameDelegate.Invoke(archiveFilename);
            ShowReadingEntriesProgressBar(archiveFilename);
        }
        
        /// <summary>
        /// Show the progress bar, indicating that the QAR entries are being read.
        /// </summary>
        /// <param name="currentArchiveFilename">
        /// Filename of the archive currently being read.
        /// </param>
        private static void ShowReadingEntriesProgressBar(string currentArchiveFilename)
        {
            EditorUtility.DisplayProgressBar(
                MakeReadingEntriesProgressBarTitleText(currentArchiveFilename),
                        ReadingEntriesText,
                        0);
        }

        /// <summary>
        /// Make the text for the progress bar title while reading archive entries.
        /// </summary>
        /// <param name="currentArchiveFilename">Filename of the archive currently being read.</param>
        /// <returns>Text to display in the progress bar title.</returns>
        private static string MakeReadingEntriesProgressBarTitleText(string currentArchiveFilename)
        {
            return string.Format(ReadingArchiveProgressBarTitle, currentArchiveFilename);
        }
        
        /// <summary>
        /// Called when a file begins being extracted.
        /// </summary>
        /// <param name="filename">
        /// Filename of the file that is being extracted.
        /// </param>
        /// <param name="totalFileCount">
        /// Total count of files to extract.
        /// </param>
        /// <param name="incrementExtractedFileCountDelegate">
        /// Delegate to increment the count of extracted files by one.
        /// </param>
        /// <param name="getExtractingArchiveFilenameDelegate">
        /// Delegate to get the filename of the archive currently being extracted.
        /// </param>
        private static void OnBeginExtractingFile(string filename, int totalFileCount, ArchiveHandler.IncrementExtractedFileCountDelegate incrementExtractedFileCountDelegate, ArchiveHandler.GetExtractingArchiveFilenameDelegate getExtractingArchiveFilenameDelegate)
        {
            var extractedFileCount = incrementExtractedFileCountDelegate.Invoke();
            var extractingArchivefilename = getExtractingArchiveFilenameDelegate.Invoke();

            ShowExtractingFileProgressBar(extractingArchivefilename, filename, extractedFileCount, totalFileCount);
        }

        /// <summary>
        /// Show the progress bar, indicating what file is currently being extracted.
        /// </summary>
        /// <param name="currentArchiveFilename">
        /// Filename of the archive currently being extracted.
        /// </param>
        /// <param name="currentFile">
        /// Filename of the file currently being extracted.
        /// </param>
        /// <param name="extractedFileCount">
        /// Number of already extracted files.
        /// </param>
        /// <param name="totalFileCount">
        /// Total number of files to extract.
        /// </param>
        private static void ShowExtractingFileProgressBar(string currentArchiveFilename, string currentFile, int extractedFileCount, int totalFileCount)
        {
            EditorUtility.DisplayProgressBar(
                        MakeExtractingFileProgressBarTitleText(),
                        string.Format(UnpackingFileText, currentFile),
                        (float)extractedFileCount / (float)totalFileCount);
        }

        /// <summary>
        /// Make the text for the progress bar title while extracting a file.
        /// </summary>
        /// <returns>Text to display in the progress bar title.</returns>
        private static string MakeExtractingFileProgressBarTitleText()
        {
            return ExtractingFilesProgressBarTitle;
        }

        /// <summary>
        /// Create streams for each archive so they can be unpacked.
        /// </summary>
        /// <param name="desiredArchiveFilenames">Filenames of the archives to unpack.</param>
        /// <param name="gameDirectory">Directory of the game files.</param>
        /// <returns>The archive streams.</returns>
        private static List<ArchiveStream> CreateArchiveStreams(List<string> desiredArchiveFilenames, string gameDirectory)
        {
            var archiveStreams = new List<ArchiveStream>();

            foreach (var archiveFilename in desiredArchiveFilenames)
            {
                var archivePath = Path.Combine(gameDirectory, archiveFilename);
                if (!File.Exists(archivePath))
                {
                    Debug.Log("Archive not found in game directory: " + archiveFilename);
                    return null;
                }

                var inputStream = new FileStream(archivePath, FileMode.Open);
                archiveStreams.Add(new ArchiveStream(inputStream, archiveFilename));
            }

            return archiveStreams;
        }

        /// <summary>
        /// Make the file format handlers.
        /// </summary>
        /// <param name="fileRegistry">
        /// Table of registered files.
        /// </param>
        /// <returns>
        /// The newly-created file format handlers.
        /// </returns>
        private static List<IFormatHandler> MakeFormatHandlers(FileRegistry fileRegistry, List<uint> routeNames, HashSet<uint> eventNames, HashSet<uint> routeMessages, HashSet<string> jsonSnippets)
        {
            var archiveHandler = new ArchiveHandler(fileRegistry);
            var routeSetHandler = new RouteSetHandler(routeNames, eventNames, routeMessages, jsonSnippets);
            var textureHandler = new TextureHandler(fileRegistry);
            var textHandler = new PlaintextHandler();
            return new List<IFormatHandler> { archiveHandler, routeSetHandler };
        }

        /// <summary>
        /// Unpacks a game's files.
        /// </summary>
        /// <param name="profile">Configuration profile for the game to unpack.</param>
        private void UnpackGame(GameProfile profile)
        {
            Assert.IsNotNull(profile, "profile must be a non-null GameProfile.");

            var gameDirectory = SelectGameDirectory();
            if (gameDirectory == string.Empty)
            {
                return;
            }

            ReadQarDictionaries(profile.QarDictionaries);
            ReadFpkDictionaries(profile.FpkDictionaries);

            var fileRegistry = new FileRegistry();
            var routeNames = new List<uint>();
            var eventNames = new HashSet<uint>();
            var routeMessages = new HashSet<uint>();
            var jsonSnippets = new HashSet<string>();
            var formatHandlers = MakeFormatHandlers(fileRegistry, routeNames, eventNames, routeMessages, jsonSnippets);

            var supportedExtensions = formatHandlers.SelectMany(handler => handler.Extensions);
            foreach (var extension in supportedExtensions)
            {
                fileRegistry.AddSupportedExtension(extension);
            }
            
            var archiveHandler = new ArchiveHandler(fileRegistry);
            var archiveStreams = CreateArchiveStreams(profile.ArchiveFiles, gameDirectory);

            UnpackArchives(archiveStreams, archiveHandler, OnBeginReadingArchive, this.SetReadingArchiveFilename);

            var extractedFilesDirectory = MakeExtractedFilesDirectory(profile.DisplayName);
            var fileExtractor = new FileExtractor(extractedFilesDirectory, formatHandlers, OnBeginExtractingFile, this.IncrementExtractedFileCount, this.GetExtractingArchiveFilename);

            fileExtractor.ExtractFiles(fileRegistry);

            foreach (var archiveStream in archiveStreams)
            {
                archiveStream.Close();
            }

            EditorUtility.ClearProgressBar();
            fileRegistry.PrintStats();

            this.ResetExtractedFileCount();


            using (var routeNameWriter = new StreamWriter(new FileStream("routeNames.txt", FileMode.Create)))
            {
                foreach (var routeName in routeNames)
                {
                    routeNameWriter.WriteLine(routeName);
                }
            }

            using (var routeEventNameWriter = new StreamWriter(new FileStream("routeEventNames.txt", FileMode.Create)))
            {
                foreach (var routeEventName in eventNames)
                {
                    routeEventNameWriter.WriteLine(routeEventName);
                }
            }

            using (var routeEventNameWriter = new StreamWriter(new FileStream("routeEventMessages.txt", FileMode.Create)))
            {
                foreach (var message in routeMessages)
                {
                    routeEventNameWriter.WriteLine(message);
                }
            }

            using (var routeEventNameWriter = new StreamWriter(new FileStream("routeJsonSnippets.txt", FileMode.Create)))
            {
                foreach (var message in jsonSnippets)
                {
                    routeEventNameWriter.WriteLine(jsonSnippets);
                }
            }
        }

        /// <summary>
        /// Increments the count of extracted files.
        /// </summary>
        /// <returns>The incremented count of extracted files.</returns>
        private int IncrementExtractedFileCount()
        {
            this.extractedFileCount++;
            return this.extractedFileCount;
        }

        /// <summary>
        /// Resets the count of extracted files.
        /// </summary>
        private void ResetExtractedFileCount()
        {
            this.extractedFileCount = 0;
        }

        /// <summary>
        /// Gets the filename of the archive currently being extracted.
        /// </summary>
        /// <returns>The filename of the archive currently being extracted.</returns>
        private string GetExtractingArchiveFilename()
        {
            return this.readingArchiveFilename;
        }

        /// <summary>
        /// Sets the filename of the archive currently being extracted.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive.</param>
        private void SetReadingArchiveFilename(string archiveFilename)
        {
            this.readingArchiveFilename = archiveFilename;
        }
    }
}