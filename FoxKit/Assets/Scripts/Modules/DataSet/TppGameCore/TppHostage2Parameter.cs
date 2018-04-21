using FoxKit.Modules.DataSet.GameCore;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    public class TppHostage2Parameter : GameObjectParameter
    {
        public string PartsFile;
        public string MotionGraphFile;
        public string MtarFile;
        public string ExtensionMtarFile;
        public List<string> VfxFiles; // TODO StringMap
    }
}