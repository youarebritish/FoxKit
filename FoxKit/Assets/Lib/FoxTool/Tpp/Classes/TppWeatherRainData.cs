using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppWeatherRainData : Data
    {
        // Static properties
        public FoxEntityLink RainFilter { get; set; }
        public FoxEntityLink FloorRainSplash { get; set; }
        public FoxFilePtr VfxFileFallRain { get; set; }
        public FoxFilePtr VfxFileFallRainSlow { get; set; }
        public FoxFilePtr VfxFileCameraFog { get; set; }
    }
}
