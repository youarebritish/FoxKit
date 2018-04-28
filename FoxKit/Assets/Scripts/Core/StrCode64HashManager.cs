namespace FoxKit.Core
{
    using FoxKit.Utils;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class StrCode64HashManager : IHashManager<ulong>
    {
        private readonly Dictionary<ulong, string> lookUpTable = new Dictionary<ulong, string>();

        public void LoadDictionary(TextAsset dictionary)
        {
            var linesInFile = dictionary.text.Split('\n');
            foreach (var line in linesInFile)
            {
                var lineWithoutNewLines = Regex.Replace(line, @"\t|\n|\r", string.Empty);
                var hash = HashString(lineWithoutNewLines);
                if (!this.lookUpTable.ContainsKey(hash))
                {
                    this.lookUpTable.Add(hash, lineWithoutNewLines);
                }
            }
        }

        public bool TryGetStringFromHash(ulong hash, out string result)
        {
            return this.lookUpTable.TryGetValue(hash, out result);
        }

        public ulong GetHash(string input)
        {
            return HashString(input);
        }

        private static ulong HashString(string input)
        {
            Assert.IsNotNull(input, "Hash input must not be null.");

            var hash = Hashing.HashFileNameLegacy(input);
            return hash;
        }

        /// <summary>
        /// Attempts to unhash a hash. If said attempt succeeds, the returned StringPair is set to string mode, if not, the returned StringPair is set to hash mode.
        /// </summary>
        /// <param name="hash">The hash to attempt to unhash.</param>
        /// <returns>The StringPair derived from the unhash attempt.</returns>
        public StrCode64StringPair GetStringPairFromUnhashAttempt(ulong hash)
        {
            return this.TryUnhash(hash, hashValue => new StrCode64StringPair(hashValue), unhashedString => new StrCode64StringPair(unhashedString));
        }

        /// <summary>
        /// Gets a hash from a string pair.
        /// </summary>
        /// <param name="stringPair">String pair.</param>
        /// <returns>The hash from string pair.</returns>
        public ulong GetHashFromStringPair(StrCode64StringPair stringPair)
        {
            return this.GetHashFromStringPair(stringPair);
        }
    }
}