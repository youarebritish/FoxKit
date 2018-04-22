using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class SoundPackage : Data
    {
        public UnityEngine.Object SoundDataFile;
        public bool SyncLoad;
    }
}