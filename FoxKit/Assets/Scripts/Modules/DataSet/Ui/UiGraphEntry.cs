using FoxKit.Modules.DataSet.FoxCore;
using System;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.Ui
{
    [Serializable]
    public class UiGraphEntry : Data
    {
        public List<UnityEngine.Object> Files;
        public List<UnityEngine.Object> RawFiles;
    }
}