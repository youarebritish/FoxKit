using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppOutOfMissionRangeEffect : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxPath LutTexture { get; set; }
        public FoxFloat StartSlope { get; set; }
        public FoxFloat EndSlope { get; set; }
        public FoxFloat BlendRatio { get; set; }
        public FoxColor ColorScale { get; set; }
        public FoxFloat NoiseScale { get; set; }
        public FoxFloat NoiseOffset { get; set; }
        public FoxFloat NoiseCutScale { get; set; }
        public FoxFloat NoiseCutOffset { get; set; }
        public FoxColor NoiseColor { get; set; }
        public FoxFloat CinemaScopeSlope { get; set; }
        public FoxFloat CinemaScopeShift { get; set; }
    }
}
