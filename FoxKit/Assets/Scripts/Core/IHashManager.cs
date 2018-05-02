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

        public static UStringPair TryUnhash<THash, UStringPair>(this IHashManager<THash> hashManager, THash hash, System.Func<THash, UStringPair> makeHashPair, System.Func<string, UStringPair> makeStringPair)
            where THash : struct 
            where UStringPair : IStringHashPair<THash>
        {
            string outString;

            if (hashManager.TryGetStringFromHash(hash, out outString) == true)
            {
                return makeStringPair(outString);
            }
            else
            {
                return makeHashPair(hash);
            }
        }

        public static UHash GetHashFromStringPair<TStringPair, UHash>(this IHashManager<UHash> hashManager, TStringPair stringPair)
            where TStringPair : IStringHashPair<UHash>
            where UHash : struct
        {
            if (stringPair.IsUnhashed == 0)
            {
                return hashManager.GetHash(stringPair.String);
            }
            else
            {
                return stringPair.Hash;
            }
        }


        public class TryUnhashResult<THash> where THash : struct
        {
            private THash hash;

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