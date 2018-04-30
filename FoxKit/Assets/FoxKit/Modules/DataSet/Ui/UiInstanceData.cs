using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils.UI.StringMap;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Ui
{
    [Serializable]
    public class UiInstanceData : Data
    {
        public ObjectStringMap CreateWindowParams;
        public List<string> WindowFactoryName;
    }
}