using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppDamageFilterExposure : Data
    {
        // Static properties
        public FoxFloat ExposureCompensation { get; set; }
        public FoxFloat MinExposure { get; set; }
        public FoxFloat MaxExposure { get; set; }
        public FoxFloat BeatExposureCompensation { get; set; }
        public FoxFloat MinBeatInterval { get; set; }
        public FoxFloat MaxBeatInterval { get; set; }
    }
}
