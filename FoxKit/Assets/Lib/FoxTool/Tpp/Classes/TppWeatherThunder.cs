using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppWeatherThunder : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat Probability { get; set; }
        public FoxFloat PowerScale { get; set; }
        public FoxFloat DecaySpeed { get; set; }
        public FoxFilePtr VfxFileStrong { get; set; }
        public FoxFilePtr VfxFileNormal { get; set; }
        public FoxFilePtr VfxFileWeak { get; set; }
        public FoxFloat FxThresholdStrong { get; set; }
        public FoxFloat FxThresholdNormal { get; set; }
        public FoxFloat FxThresholdWeak { get; set; }
        public FoxBool EnableSpotLight { get; set; }
        public FoxFloat SpotLightLifeMin { get; set; }
        public FoxFloat SpotLightLifeMax { get; set; }
        public FoxFloat SpotLightLumen { get; set; }
    }
}
