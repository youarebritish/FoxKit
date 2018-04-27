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
        public delegate TryUnhashResult<THash> TryUnhashDelegate<THash>(THash hash) where THash : struct;

        public static TryUnhashDelegate<THash> MakeUnhashFunc<THash>(this IHashManager<THash> hashManager) where THash : struct
        {
            return (hash =>
            {
                string result;
                if (!hashManager.TryGetStringFromHash(hash, out result))
                {
                    return new TryUnhashResult<THash>(hash);
                }
                return new TryUnhashResult<THash>(result);
            });
        }

        public class TryUnhashResult<THash> where THash : struct
        {
            private THash hash;
            private THash hash1;
            
            public bool WasNameUnhashed { get; }
            public string UnhashedString { get; }
            public THash Hash { get; }

            public TryUnhashResult(string unhashedName)
            {
                WasNameUnhashed = true;
                UnhashedString = unhashedName;
                Hash = default(THash);
            }

            public TryUnhashResult(THash hash)
            {
                WasNameUnhashed = false;
                UnhashedString = null;
                Hash = hash;
            }
        }
    }
}