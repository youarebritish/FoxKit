namespace FoxKit.Core
{
    [System.Serializable]
    public class PathFileNameCode64StringPair : IStringHashPair<ulong>
    {
        public string String => _string;
        public ulong Hash => System.Convert.ToUInt64(_hash);/*_hash;*/ //Unity is dumb and their PropertyFields don't support ulongs
        public IsStringOrHash IsUnhashed => _isUnhashed;


        private string _string;

        private /*ulong*/string _hash;

        private IsStringOrHash _isUnhashed;

        public PathFileNameCode64StringPair(string /*@string*/name, IsStringOrHash hashState) //This should be avoidable
        {
            if (hashState == IsStringOrHash.String)
            {
                this._string = name;
                this._hash = null;
            }
            else
            {
                this._string = null/*@string*/;
                this._hash = name;
            }

            this._isUnhashed = hashState;//IsStringOrHash.String;
        }

        //public PathFileNameCode64StringPair(ulong hash)
        //{
        //    this._string = null;
        //    this._hash = hash;
        //    this._isUnhashed = IsStringOrHash.Hash;
        //}
    }
}