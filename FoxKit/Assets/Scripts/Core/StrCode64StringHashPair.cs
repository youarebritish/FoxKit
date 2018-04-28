namespace FoxKit.Core
{
    [System.Serializable]
    public class StrCode64StringPair : IStringHashPair<ulong>
    {
        public string String => _string;
        public ulong Hash => _hash;
        public bool IsUnhashed => _isUnhashed;

        [UnityEngine.SerializeField]
        private string _string;
        [UnityEngine.SerializeField]
        private ulong _hash;
        [UnityEngine.SerializeField]
        private bool _isUnhashed;

        public StrCode64StringPair(string @string)
        {
            this._string = @string;
            this._isUnhashed = true;
            this._hash = ulong.MaxValue;
        }

        public StrCode64StringPair(ulong hash)
        {
            this._string = null;
            this._hash = hash;
            this._isUnhashed = false;
        }
    }
}