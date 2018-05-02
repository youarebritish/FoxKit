namespace FoxKit.Core
{
    [System.Serializable]
    public class StrCode64StringPair : IStringHashPair<ulong>
    {
        public string String => _string;
        public ulong Hash => _hash;
        public IsStringOrHash IsUnhashed => _isUnhashed;

        [UnityEngine.SerializeField, OneLine.Width(200)]
        private string _string;
        [UnityEngine.SerializeField, OneLine.Width(200)]
        private ulong _hash;
        [UnityEngine.SerializeField, OneLine.Width(50)]
        private IsStringOrHash _isUnhashed;

        public StrCode64StringPair(string @string)
        {
            this._string = @string;
            this._hash = ulong.MaxValue;
            this._isUnhashed = IsStringOrHash.String;
        }

        public StrCode64StringPair(ulong hash)
        {
            this._string = null;
            this._hash = hash;
            this._isUnhashed = IsStringOrHash.Hash;
        }
    }
}