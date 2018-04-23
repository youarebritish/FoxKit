using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppRatLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxFloat FadeTime { get; set; }
        public FoxString RouteId { get; set; }
        public FoxString EscapeRouteId { get; set; }
        public FoxEntityLink AreaLocator { get; set; }
        public FoxEntityLink EscapePoint { get; set; }
        public FoxString NoiseName { get; set; }
        public FoxString NoiseTagName { get; set; }
        public FoxFloat NoiseRange { get; set; }
        public FoxInt32 NoisePowerThreshold { get; set; }
    }
}
