using FoxKit.Modules.DataSet.FoxCore;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Ui
{
    [Serializable]
    public class UiGraphEntry : Data
    {
        public List<string> Files;
        public List<string> RawFiles;
    }
}