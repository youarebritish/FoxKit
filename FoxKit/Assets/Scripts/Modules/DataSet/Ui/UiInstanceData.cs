using FoxKit.Modules.DataSet.FoxCore;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Ui
{
    [Serializable]
    public class UiInstanceData : Data
    {
        public List<string> CreateWindowParams; // TODO: StringMap
        public List<string> WindowFactoryName;
    }
}