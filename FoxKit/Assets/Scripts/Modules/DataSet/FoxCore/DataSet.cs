using FoxKit.Utils.UI.StringMap;
using System;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class DataSet : Data
    {
        public EntityStringMap DataList = new EntityStringMap();
    }
}