namespace FoxKit.Core
{
    [System.Serializable]
    public class StrCode32StringPair : IStringHashPair<uint>
    {
        public string String => _string;
        public uint Hash => _hash;
        public IsStringOrHash IsUnhashed => _isUnhashed;

        private string _string;

        private uint _hash;

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