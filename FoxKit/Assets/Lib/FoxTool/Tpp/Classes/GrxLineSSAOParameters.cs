using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GrxLineSSAOParameters
    {
        // Static properties
        public FoxFloat InnerRadius { get; set; }
        public FoxFloat OuterRadius { get; set; }
        public FoxFloat MaxDistanceInner { get; set; }
        public FoxFloat MaxDistanceOuter { get; set; }
        public FoxFloat Contrast { get; set; }
        public FoxInt32 BlurMode { get; set; }
        public FoxFloat BlurRadius { get; set; }
        public FoxFloat FalloffStart { get; set; }
        public FoxFloat FalloffRange { get; set; }
        public FoxFloat GainonStart { get; set; }
        public FoxFloat GainonRange { get; set; }
    }
}
