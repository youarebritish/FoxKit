using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppBirdLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxFloat FadeoutTime { get; set; }
        public FoxFloat FlyTime { get; set; }
        public FoxEntityLink SkyLocator { get; set; }
        public FoxEntityLink GroundLocators { get; set; }
        public FoxEntityLink PerchesOnEdges { get; set; }
        public FoxString NoiseName { get; set; }
        public FoxString NoiseTagName { get; set; }
        public FoxFloat NoiseRange { get; set; }
        public FoxInt32 NoisePowerThreshold { get; set; }
    }
}
