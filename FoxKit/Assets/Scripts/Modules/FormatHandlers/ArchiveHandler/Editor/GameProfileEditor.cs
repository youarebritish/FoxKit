namespace FoxKit.Modules.FormatHandlers.ArchiveHandler.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Core;
    using FoxKit.Modules.FormatHandlers.PlaintextHandler;

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
        /// Text to display in the title of the game unpacking progress bar.
        /// </summary>
        private const string UnpackingGameProgressBarTitle = "Extracting {0}...";

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
        /// Filename ofthe archive currently being extracted.
        /// </summary>
        private string extractingArchiveFilename;
        #endregion

        #region Delegates
        /// <summary>
        /// Delegate for a callback invoked when an archive begins being extracted.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive beginning extraction.</param>
        /// <param name="resetExtractedFileCountDelegate">Delegate to reset the count of extracted files.</param>
        /// <param name="setExtractingArchiveFilenameDelegate">Delegate to set the filename of the extracting archive.</param>
        private delegate void BeginExtractingArchiveDelegate(string archiveFilename, ArchiveHandler.ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, ArchiveHandler.SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate);
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
        /// <param name="onBeginExtractingArchive">
        /// Callback to invoke when an archive begins being extracted.
        /// </param>
        /// <param name="resetExtractedFileCountDelegate">
        /// Delegate to reset the count of extracted files.
        /// </param>
        /// <param name="setExtractingArchiveFilenameDelegate">
        /// Delegate to set the name of the extracting archive.
        /// </param>
        private static void UnpackArchives(ICollection<ArchiveStream> archiveStreams, ArchiveHandler archiveHandler, BeginExtractingArchiveDelegate onBeginExtractingArchive, ArchiveHandler.ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, ArchiveHandler.SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate)
        {
            Assert.IsNotNull(archiveStreams, "archiveStreams must not be null.");
            Assert.IsNotNull(archiveHandler, "archiveHandler must not be null.");
            Assert.IsNotNull(onBeginExtractingArchive, "onBeginExtractingArchive must not be null.");
            Assert.IsNotNull(resetExtractedFileCountDelegate, "resetExtractedFileCountDelegate must not be null.");
            Assert.IsNotNull(setExtractingArchiveFilenameDelegate, "setExtractingArchiveFilenameDelegate must not be null.");

            foreach (var archiveStream in archiveStreams)
            {
                onBeginExtractingArchive.Invoke(archiveStream.Path, resetExtractedFileCountDelegate, setExtractingArchiveFilenameDelegate);
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
        /// /// <param name="resetExtractedFileCountDelegate">Delegate to reset the count of extracted files.</param>
        /// /// <param name="setExtractingArchiveFilenameDelegate">Delegate to set the filename of the archive being extracted.</param>
        private static void OnBeginExtractingArchive(string archiveFilename, ArchiveHandler.ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, ArchiveHandler.SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate)
        {
            resetExtractedFileCountDelegate.Invoke();
            setExtractingArchiveFilenameDelegate.Invoke(archiveFilename);
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
                        MakeProgressBarTitleText(currentArchiveFilename),
                        ReadingEntriesText,
                        0);
        }
        
        /// <summary>
        /// Make the text for the progress bar title.
        /// </summary>
        /// <param name="currentArchiveFilename">Filename of the archive currently being extracted.</param>
        /// <returns>Text to display in the progress bar title.</returns>
        private static string MakeProgressBarTitleText(string currentArchiveFilename)
        {
            return string.Format(UnpackingGameProgressBarTitle, currentArchiveFilename);
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
                        MakeProgressBarTitleText(currentArchiveFilename),
                        string.Format(UnpackingFileText, currentFile),
                        (float)extractedFileCount / (float)totalFileCount);
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
        private static List<IFormatHandler> MakeFormatHandlers(FileRegistry fileRegistry)
        {
            var archiveHandler = new ArchiveHandler(fileRegistry);
            var textHandler = new PlaintextHandler();
            return new List<IFormatHandler> { textHandler, archiveHandler };
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
            var formatHandlers = MakeFormatHandlers(fileRegistry);

            var supportedExtensions = new HashSet<string>(formatHandlers.SelectMany(handler => handler.Extensions));
            foreach (var extension in supportedExtensions)
            {
                fileRegistry.AddSupportedExtension(extension);
            }
            
            var archiveHandler = new ArchiveHandler(fileRegistry);
            var archiveStreams = CreateArchiveStreams(profile.ArchiveFiles, gameDirectory);

            UnpackArchives(archiveStreams, archiveHandler, OnBeginExtractingArchive, this.ResetExtractedFileCount, this.SetExtractingArchiveFilename);

            var extractedFilesDirectory = MakeExtractedFilesDirectory(profile.DisplayName);
            var fileExtractor = new FileExtractor(extractedFilesDirectory, formatHandlers, OnBeginExtractingFile, this.IncrementExtractedFileCount, this.GetExtractingArchiveFilename);

            fileExtractor.ExtractFiles(fileRegistry);

            foreach (var archiveStream in archiveStreams)
            {
                archiveStream.Close();
            }

            EditorUtility.ClearProgressBar();

            fileRegistry.PrintStats();
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
            return this.extractingArchiveFilename;
        }

        /// <summary>
        /// Sets the filename of the archive currently being extracted.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive.</param>
        private void SetExtractingArchiveFilename(string archiveFilename)
        {
            this.extractingArchiveFilename = archiveFilename;
        }
    }
}