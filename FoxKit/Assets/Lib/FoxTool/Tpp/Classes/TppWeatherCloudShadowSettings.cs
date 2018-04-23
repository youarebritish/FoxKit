using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppWeatherCloudShadowSettings : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxPath Texture { get; set; }
        public FoxFloat TextureMappingRange { get; set; }
        public FoxFloat WindInfluence { get; set; }
    }
}
