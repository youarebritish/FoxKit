using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GrxAreaSSAOParameters
    {
        // Static properties
        public FoxInt32 Resolution { get; set; }
        public FoxFloat Radius { get; set; }
        public FoxFloat AngleBias { get; set; }
        public FoxUInt32 NumSteps { get; set; }
        public FoxUInt32 NumDirections { get; set; }
        public FoxFloat Attenuation { get; set; }
        public FoxFloat Contrast { get; set; }
        public FoxInt32 BlurMode { get; set; }
        public FoxFloat BlurRadius { get; set; }
        public FoxFloat BlurSharpness { get; set; }
        public FoxFloat BlurSceneScale { get; set; }
    }
}
