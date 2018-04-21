using FoxKit.Modules.DataSet.FoxCore;
using System;

namespace FoxKit.Modules.DataSet.TppGameKit
{
    /// <summary>
    /// Note: Gimmicks are weird. They're Data, not TransformData, and get their transform from an lba.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickData : Data
    {
        public string PartsFile;
        public string LocatorFile;
        public TppPermanentGimmickParameter Parameters;
        public uint Flags1;         // TODO enum
        public uint Flags2;         // TODO enum
    }
}