namespace FoxKit.Core
{
    [System.Serializable]
    public class StrCode32StringPair : IStringHashPair<uint>
    {
        public string String => _string;
        public uint Hash => _hash;
        public bool IsUnhashed => _isUnhashed;

        [UnityEngine.SerializeField]
        private string _string;
        [UnityEngine.SerializeField]
        private uint _hash;
        [UnityEngine.SerializeField]
        private bool _isUnhashed;

        public StrCode32StringPair(string @string)
        {
            this._string = @string;
            this._isUnhashed = true;
            this._hash = uint.MaxValue;
        }

        public StrCode32StringPair(uint hash)
        {
            this._string = null;
            this._hash = hash;
            this._isUnhashed = false;
        }
    }
}