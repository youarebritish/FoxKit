namespace FoxKit.Core
{
    [System.Serializable]
    public class StrCode32StringPair : IStringHashPair<uint>
    {
        public string String => _string;
        public uint Hash => _hash;
        public IsStringOrHash IsUnhashed => _isUnhashed;

        [UnityEngine.SerializeField, OneLine.Width(200)]
        private string _string;
        [UnityEngine.SerializeField, OneLine.Width(200)]
        private uint _hash;
        [UnityEngine.SerializeField, OneLine.Width(50)]
        private IsStringOrHash _isUnhashed;

        public StrCode32StringPair(string @string)
        {
            this._string = @string;
            this._hash = uint.MaxValue;
            this._isUnhashed = IsStringOrHash.String;
        }

        public StrCode32StringPair(uint hash)
        {
            this._string = null;
            this._hash = hash;
            this._isUnhashed = IsStringOrHash.Hash;
        }
    }
}