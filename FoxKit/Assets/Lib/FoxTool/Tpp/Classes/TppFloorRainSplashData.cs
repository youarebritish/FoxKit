using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppFloorRainSplashData : Data
    {
        // Static properties
        public FoxFilePtr VfxFile { get; set; }
        public Dictionary<string, FoxString> MaterialSoundList { get; set; }
    }
}
