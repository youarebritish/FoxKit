using UnityEngine;

namespace FoxKit.Core
{
    public interface IHashManager<THash>
        where THash : struct
    {
        void LoadDictionary(TextAsset dictionary);

        bool TryGetStringFromHash(THash hash, out string result);

        THash GetHash(string input);
    }    

    public static class IHashManagerExtensions
    {
        public delegate TryUnhashResult TryUnhashDelegate(uint hash);

        public static TryUnhashDelegate MakeUnhashFunc(this IHashManager<uint> hashManager)
        {
            return (hash =>
            {
                string result;
                if (!hashManager.TryGetStringFromHash(hash, out result))
                {
                    return new TryUnhashResult(hash);
                }
                return new TryUnhashResult(result);
            });
        }

        public class TryUnhashResult
        {
            public bool WasNameUnhashed { get; }
            public string UnhashedString { get; }
            public uint Hash { get; }

            public TryUnhashResult(string unhashedName)
            {
                WasNameUnhashed = true;
                UnhashedString = unhashedName;
                Hash = uint.MaxValue;
            }

            public TryUnhashResult(uint hash)
            {
                WasNameUnhashed = false;
                UnhashedString = null;
                Hash = hash;
            }
        }
    }
}