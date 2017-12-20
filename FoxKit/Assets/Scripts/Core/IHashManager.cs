using UnityEngine;

namespace FoxKit.Core
{
    public interface IHashManager<THash>
        where THash : struct
    {
        void LoadDictionary(TextAsset dictionary);

        bool TryGetStringFromHash(THash hash, out string result);
    }
}