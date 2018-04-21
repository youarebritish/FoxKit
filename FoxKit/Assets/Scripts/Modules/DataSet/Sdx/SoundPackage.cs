using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class SoundPackage : Data
    {
        public string SoundDataFile;    // TODO FilePtr
        public bool SyncLoad;
    }
}