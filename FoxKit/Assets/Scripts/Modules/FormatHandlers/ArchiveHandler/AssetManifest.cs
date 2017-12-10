using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;

namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    /// <summary>
    /// Database of extracted assets, which can be queried to determine if a given asset has been modified since it was extracted.
    /// </summary>
    public class AssetManifest : ScriptableObject
    {
        #region Fields
        /// <summary>
        /// Registered asset entries.
        /// </summary>
        private readonly List<Entry> Entries = new List<Entry>();

        /// <summary>
        /// MD5 hashing algorithm.
        /// </summary>
        private readonly HashAlgorithm HashAlgorithm = MD5.Create();
        #endregion

        /// <summary>
        /// Registers an asset so it can later be checked for modifications.
        /// </summary>
        /// <param name="filename">Filename of the asset to register.</param>
        /// <param name="contents">Contents of the asset.</param>
        public void RegisterAsset(string filename, Stream contents)
        {
            Assert.IsNotNull(contents, "Stream contents must not be null.");

            var entry = new Entry(filename, ComputeChecksum(contents, HashAlgorithm));
            Entries.Add(entry);
        }

        /// <summary>
        /// Determines whether or not an asset has been modified since it was unpacked.
        /// </summary>
        /// <param name="filename">Filename of the asset to check.</param>
        /// <param name="contents">Contents of the asset to check.</param>
        /// <returns>True if the asset has been modified, else false.</returns>
        public bool WasAssetModified(string filename, Stream contents)
        {
            var entry = FindEntry(filename, Entries);

            if (entry == null)
            {
                return false;
            }

            var checksum = ComputeChecksum(contents, HashAlgorithm);
            return !(IsHashIdentical(entry.Checksum, checksum));
        }

        /// <summary>
        /// Computes the checksum for an asset.
        /// </summary>
        /// <param name="asset">Stream of the asset's contents.</param>
        /// <param name="hashAlgorithm">Hash algorithm to use for computing the checksum.</param>
        /// <returns>The asset's checksum.</returns>
        private static byte[] ComputeChecksum(Stream asset, HashAlgorithm hashAlgorithm)
        {            
            Assert.IsNotNull(hashAlgorithm, "hashAlgorithm must not be null.");

            return hashAlgorithm.ComputeHash(asset);
        }

        /// <summary>
        /// Compares two hashes.
        /// </summary>
        /// <param name="hashA">One hash.</param>
        /// <param name="hashB">Another hash.</param>
        /// <returns>True ff hashA and hashB are identical, else false.</returns>
        private static bool IsHashIdentical(byte[] hashA, byte[] hashB)
        {
            if (hashB.Length == hashA.Length)
            {
                int i = 0;
                while ((i < hashB.Length) && (hashB[i] == hashA[i]))
                {
                    i += 1;
                }
                if (i == hashB.Length)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the registered asset entry with the given filename.
        /// </summary>
        /// <param name="filename">Filename of the asset entry to find.</param>
        /// <param name="entries">Registered asset entry list.</param>
        /// <returns>The entry with the given filename, or null if it hasn't been registered.</returns>
        private static Entry FindEntry(string filename, IEnumerable<Entry> entries)
        {
            Assert.IsNotNull(entries, "Entry list must not be null.");

            var result = entries.First(entry => entry.Filename == filename);
            return result;
        }

        /// <summary>
        /// A registered asset entry.
        /// </summary>
        [Serializable]
        private class Entry
        {
            public readonly string Filename;
            public readonly byte[] Checksum;

            public Entry(string filename, byte[] checksum)
            {
                Filename = filename;
                Checksum = checksum;
            }
        }
    }
}