namespace FoxKit.Core.WIP
{
    using System;
    using OdinSerializer;

    // I tried to use generics here, but because of the non-standard int/ulong.Format(string format) it quickly became a Generic mess.

    [Serializable]
    public sealed class Str32CodeHashPair
    {
        [OdinSerialize]
        private string @string;

        [OdinSerialize]
        private uint hash;

        [OdinSerialize]
        private bool isHash;

        private static readonly Func<string, uint> hashFunc = StrCode32HashManager.HashString;

        public static implicit operator string(Str32CodeHashPair value)
        {
            if (value.isHash || value.@string == null)
            {
                return value.hash.ToString("x");
            }
            else
            {
                return value.@string;
            }
        }
        public static implicit operator Str32CodeHashPair(string value)
        {
            var output = new Str32CodeHashPair
            {
                @string = value
            };
            output.hash = GetHash(output.@string);
            output.isHash = false;

            if (output.isHash)
            {
                output.@string = null;
            }
            else
            {
                output.@string = value;
            }

            return output;
        }

        public static implicit operator uint(Str32CodeHashPair value)
        {
            return value.hash;
        }
        public static implicit operator Str32CodeHashPair(uint value)
        {
            var output = new Str32CodeHashPair
            {
                hash = value,
                isHash = true, // We take the caller's "word" that this is a hash; no pseudo-hash would be passed as a uint.
                @string = null
            };

            return output;
        }

        private static uint GetHash(string value)
        {
            return hashFunc(value);
        }

        private static bool IsHash(uint hash)
        {
            return Dictionaries.Str32CodeHashDictionary.ContainsValue(hash);
        }

        public void TryUnhashString(Func<uint, string> unhashFunc)
        {
            if (isHash && @string == null)
            {
                @string = unhashFunc(hash);
            }
        }
    }

    [Serializable]
    public sealed class PathFileNameCode64HashPair
    {
        [OdinSerialize]
        private string @string;

        [OdinSerialize]
        private ulong hash;

        [OdinSerialize]
        private bool isHash;

        private static readonly Func<string, ulong> hashFunc = PathFileNameCode64HashManager.HashString;

        public static implicit operator string(PathFileNameCode64HashPair value)
        {
            if (value.isHash || value.@string == null)
            {
                return value.hash.ToString("x");
            }
            else
            {
                return value.@string;
            }
        }
        public static implicit operator PathFileNameCode64HashPair(string value)
        {
            var output = new PathFileNameCode64HashPair();
            output.@string = value;
            output.hash = GetHash(output.@string);
            output.isHash = false;

            if (output.isHash)
            {
                output.@string = null;
            }
            else
            {
                output.@string = value;
            }

            return output;
        }

        public static implicit operator ulong(PathFileNameCode64HashPair value)
        {
            return value.hash;
        }
        public static implicit operator PathFileNameCode64HashPair(ulong value)
        {
            var output = new PathFileNameCode64HashPair();
            output.hash = value;
            output.isHash = IsHash(value);
            output.@string = null;

            return output;
        }

        private static ulong GetHash(string value)
        {
            return hashFunc(value);
        }

        private static bool IsHash(ulong hash)
        {
            return Dictionaries.PathFileNameCode64HashDictionary.ContainsValue(hash);
        }

        public void TryUnhashString(Func<ulong, string> unhashFunc)
        {
            if (isHash && @string != null)
                @string = unhashFunc(hash);
        }
    }
}