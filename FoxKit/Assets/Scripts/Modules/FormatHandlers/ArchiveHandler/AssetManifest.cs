namespace FoxKit.Modules.FormatHandlers.ArchiveHandler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Database of extracted assets, which can be queried to determine if a given asset has been modified since it was extracted.
    /// </summary>
    public class AssetManifest : ScriptableObject
    {
        /// <summary>
        /// Registered asset entries.
        /// </summary>
        private readonly List<Entry> entries = new List<Entry>();

        /// <summary>
        /// MD5 hashing algorithm.
        /// </summary>
        private readonly HashAlgorithm hashAlgorithm = MD5.Create();

        /// <summary>
        /// Registers an asset so it can later be checked for modifications.
        /// </summary>
        /// <param name="filename">Filename of the asset to register.</param>
        /// <param name="contents">Contents of the asset.</param>
        public void RegisterAsset(string filename, Stream contents)
        {
            Assert.IsNotNull(contents, "Stream contents must not be null.");

            var entry = new Entry(filename, ComputeChecksum(contents, this.hashAlgorithm));
            this.entries.Add(entry);
        }

        /// <summary>
        /// Determines whether or not an asset has been modified since it was unpacked.
        /// </summary>
        /// <param name="filename">Filename of the asset to check.</param>
        /// <param name="contents">Contents of the asset to check.</param>
        /// <returns>True if the asset has been modified, else false.</returns>
        public bool WasAssetModified(string filename, Stream contents)
        {
            var entry = FindEntry(filename, this.entries);

            if (entry == null)
            {
                return false;
            }

            var checksum = ComputeChecksum(contents, this.hashAlgorithm);
            return !IsHashIdentical(entry.Checksum, checksum);
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
        /// <returns>True if hashA and hashB are identical, else false.</returns>
        private static bool IsHashIdentical(IReadOnlyList<byte> hashA, IReadOnlyList<byte> hashB)
        {
            if (hashB.Count != hashA.Count)
            {
                return false;
            }

            var i = 0;
            while (i < hashB.Count && hashB[i] == hashA[i])
            {
                i += 1;
            }

            return i == hashB.Count;
        }

        /// <summary>
        /// Finds the registered asset entry with the given filename.
        /// </summary>
        /// <param name="filename">Filename of the asset entry to find.</param>
        /// <param name="entries">Registered asset entry list.</param>
        /// <returns>The entry with the given filename, or null if it hasn't been registered.</returns>
        private static Entry FindEntry(string filename, ICollection<Entry> entries)
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
            /// <summary>
            /// The filename.
            /// </summary>
            public readonly string Filename;

            /// <summary>
            /// The checksum.
            /// </summary>
            public readonly byte[] Checksum;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> class.
            /// </summary>
            /// <param name="filename">
            /// The filename.
            /// </param>
            /// <param name="checksum">
            /// The checksum.
            /// </param>
            public Entry(string filename, byte[] checksum)
            {
                this.Filename = filename;
                this.Checksum = checksum;
            }
        }
    }
}