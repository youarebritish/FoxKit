namespace FoxKit.Core
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using GzsTool.Core.Utility;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class StrCode32HashManager : IHashManager<uint>
    {
        private readonly Dictionary<uint, string> lookUpTable = new Dictionary<uint, string>();

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

        public bool TryGetStringFromHash(uint hash, out string result)
        {
            return this.lookUpTable.TryGetValue(hash, out result);
        }

        private static uint HashString(string input)
        {
            Assert.IsNotNull(input, "Hash input must not be null.");

            var hash = (uint)Hashing.HashFileNameLegacy(input);
            return hash;
        }
    }
}