using FoxKit.Core;
using GzsTool.Core.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static FoxKit.Modules.FormatHandlers.ArchiveHandler.ArchiveHandler;

namespace FoxKit.Modules.FormatHandlers.ArchiveHandler.Editor
{
    [CustomEditor(typeof(GameProfile))]
    public class GameProfileEditor : UnityEditor.Editor
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
        int ExtractedFileCount;

        /// <summary>
        /// Filename ofthe archive currently being extracted.
        /// </summary>
        string ExtractingArchiveFilename;
        #endregion
        
        #region Delegates
        /// <summary>
        /// Delegate for a callback invoked when an archive begins being extracted.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive beginning extraction.</param>
        private delegate void BeginExtractingArchiveDelegate(string archiveFilename, ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate);
        #endregion

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button(UnpackButtonLabel))
            {
                UnpackGame(target as GameProfile);
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
            return EditorUtility.OpenFolderPanel(SelectGameDirectoryPanelTitle, "", "");
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
        /// Read the QAR dictionaries so that filenames can be unhashed.
        /// </summary>
        /// <param name="qarDictionaries">List of QAR dictionaries to load.</param>
        /// <returns>The total number of files in the dictionaries.</returns>
        private static void ReadQarDictionaries(List<TextAsset> qarDictionaries)
        {
            Assert.IsNotNull(qarDictionaries, "qarDictionaries must not be null.");

            foreach (var dictionary in qarDictionaries)
            {
                Hashing.ReadDictionary(dictionary);
            }
        }

        /// <summary>
        /// Unpack game archives.
        /// </summary>
        /// <param name="desiredArchiveFilenames">Filenames of the archives to extract.</param>
        /// <param name="gameDirectory">Directory to the game to extract.</param>
        /// <param name="archiveHandler">Format handler for archives.</param>
        private static void UnpackArchives(List<ArchiveStream> archiveStreams, string gameDirectory, ArchiveHandler archiveHandler, BeginExtractingArchiveDelegate onBeginExtractingArchive, ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate)
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
        /// <param name="archivePath">Path to the archive to be extracted.</param>
        /// <param name="archiveHandler">Format handler for archives.</param>
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
        private static void OnBeginExtractingArchive(string archiveFilename, ResetExtractedFileCountDelegate resetExtractedFileCountDelegate, SetExtractingArchiveFilenameDelegate setExtractingArchiveFilenameDelegate)
        {
            resetExtractedFileCountDelegate.Invoke();
            setExtractingArchiveFilenameDelegate.Invoke(archiveFilename);
            ShowReadingEntriesProgressBar(archiveFilename);
        }
        
        /// <summary>
        /// Show the progress bar, indicating that the QAR entries are beign read
        /// </summary>
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
        /// <param name="filename">Filename of the file that is being extracted.</param>
        private static void OnBeginExtractingFile(string filename, int totalFileCount, IncrementExtractedFileCountDelegate incrementExtractedFileCountDelegate, GetExtractingArchiveFilenameDelegate getExtractingArchiveFilenameDelegate)
        {
            var extractedFileCount = incrementExtractedFileCountDelegate.Invoke();
            var extractingArchivefilename = getExtractingArchiveFilenameDelegate.Invoke();

            ShowExtractingFileProgressBar(extractingArchivefilename, filename, extractedFileCount, totalFileCount);
        }

        /// <summary>
        /// Show the progress bar, indicating what file is currently being extracted.
        /// </summary>
        /// <param name="currentFile">Filename of the file currently being extracted.</param>
        /// <param name="extractedFileCount">Number of already extracted files.</param>
        /// <param name="totalFileCount">Total number of files to extract.</param>
        private static void ShowExtractingFileProgressBar(string currentArchiveFilename, string currentFile, int extractedFileCount, int totalFileCount)
        {
            EditorUtility.DisplayProgressBar(
                        MakeProgressBarTitleText(currentArchiveFilename),
                        string.Format(UnpackingFileText, currentFile),
                        (float)extractedFileCount / (float)totalFileCount);
        }

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
        /// Unpacks a game's files.
        /// </summary>
        /// <param name="profile">Configuration profile for the game to unpack.</param>
        private void UnpackGame(GameProfile profile)
        {
            Assert.IsNotNull(profile, "profile must be a non-null GameProfile.");

            var gameDirectory = SelectGameDirectory();
            if (gameDirectory == string.Empty) return;

            ReadQarDictionaries(profile.QarDictionaries);

            var textHandler = new PlaintextHandler();
            var formatHandlers = new List<IFormatHandler>() { textHandler };
            var supportedExtensions = new HashSet<string>(formatHandlers.SelectMany(handler => handler.Extensions));
            
            var fileRegistry = new FileRegistry(supportedExtensions);
            var extractedFilesDirectory = MakeExtractedFilesDirectory(profile.DisplayName);

            var archiveHandler = new ArchiveHandler(fileRegistry);
            var archiveStreams = CreateArchiveStreams(profile.ArchiveFiles, gameDirectory);

            UnpackArchives(archiveStreams, gameDirectory, archiveHandler, OnBeginExtractingArchive, ResetExtractedFileCount, SetExtractingArchiveFilename);            
            
            var fileExtractor = new FileExtractor(extractedFilesDirectory, formatHandlers, OnBeginExtractingFile, IncrementExtractedFileCount, GetExtractingArchiveFilename);

            fileExtractor.ExtractFiles(fileRegistry);

            foreach(var archiveStream in archiveStreams)
            {
                archiveStream.Close();
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Increments the count of extracted files.
        /// </summary>
        /// <returns>The incremented count of extracted files.</returns>
        private int IncrementExtractedFileCount()
        {
            ExtractedFileCount++;
            return ExtractedFileCount;
        }

        /// <summary>
        /// Resets the count of extraced files.
        /// </summary>
        private void ResetExtractedFileCount()
        {
            ExtractedFileCount = 0;
        }

        /// <summary>
        /// Gets the filename of the archive currently being extracted.
        /// </summary>
        /// <returns>The filename of the archive currently being extracted.</returns>
        private string GetExtractingArchiveFilename()
        {
            return ExtractingArchiveFilename;
        }

        /// <summary>
        /// Sets the filename of the archive currently being extracted.
        /// </summary>
        /// <param name="archiveFilename">Filename of the archive.</param>
        private void SetExtractingArchiveFilename(string archiveFilename)
        {
            ExtractingArchiveFilename = archiveFilename;
        }
    }
}