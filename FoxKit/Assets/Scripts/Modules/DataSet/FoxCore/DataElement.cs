using System;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public abstract class DataElement<TOwner> : Entity where TOwner : Entity
    {
        public TOwner Owner;
    }
}