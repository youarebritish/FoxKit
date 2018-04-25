using FoxKit.Utils.UI.StringMap;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public class DataSet : Data
    {
        [SerializeField]
        public EntityStringMap DataList = new EntityStringMap();
    }
}