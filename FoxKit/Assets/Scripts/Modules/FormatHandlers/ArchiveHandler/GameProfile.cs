namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    using System.Collections.Generic;

    using FoxKit.Utils;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Algorithm to use for hashing string literals and filenames.
    /// </summary>
    public enum HashingAlgorithm
    {
        /// <summary>
        /// Used in The Phantom Pain.
        /// </summary>
        Tpp,
        /// <summary>
        /// Used in Ground Zeroes.
        /// </summary>
        Gz
    }

    /// <inheritdoc />
    /// <summary>
    /// Configuration data for a Fox Engine game to unpack/repack. Tells the ArchiveHandler which archives to search for and which algorithms to use.
    /// </summary>
    public class GameProfile : ScriptableObject
    {
        /// <summary>
        /// Name of the profile to display in the "Unpack Game" menu.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// Which string hashing algorithm to use.
        /// </summary>
        public HashingAlgorithm HashingAlgorithm = HashingAlgorithm.Tpp;

        /// <summary>
        /// Filenames of archives to search for and unpack. Archives will be unpacked in the order that they're listed here.
        /// </summary>
        public List<string> ArchiveFiles;

        /// <summary>
        /// The QAR (archive) dictionaries.
        /// </summary>
        public List<TextAsset> QarDictionaries;

        /// <summary>
        /// The fpk/fpkd dictionaries.
        /// </summary>
        public List<TextAsset> FpkDictionaries;

        [MenuItem("Assets/Create/FoxKit/Game Profile")]
        public static void CreateAsset()
        {
            CreateScriptableObject.CreateAsset<GameProfile>();
        }
    }
}