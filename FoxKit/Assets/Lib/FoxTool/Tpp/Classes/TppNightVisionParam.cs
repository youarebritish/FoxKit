using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppNightVisionParam
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxBool Enable { get; set; }
        public FoxPath ColorCorrectionLUT { get; set; }
        public FoxFloat ExposureCompensation { get; set; }
        public FoxFloat SwitchOnCompensation { get; set; }
        public FoxFloat SwitchOnEffectTime { get; set; }
        public FoxFloat SwitchOffCompensation { get; set; }
        public FoxFloat SwitchOffEffectTime { get; set; }
        public FoxFloat TonemapThreshold { get; set; }
        public FoxFloat TonemapRange { get; set; }
    }
}
