using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class AmbientOcclusionSettings : Data
    {
        // Static properties
        public FoxInt32 Method { get; set; }
        public FoxInt32 Attachment { get; set; }
        // name=lineSSAOParameters
        public FoxEntityPtr LineSsaoParameters { get; set; }
        // name=areaSSAOParameters
        public FoxEntityPtr AreaSsaoParameters { get; set; }
    }
}
