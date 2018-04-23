using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class WindParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxFloat DecayRate { get; set; }
        public FoxVector3 Velocity { get; set; }
        public FoxFloat SpeedTurbulentRate { get; set; }
        public FoxFloat SpeedTurbulentCycle { get; set; }
        public FoxFloat RotTurbulentRate { get; set; }
        public FoxFloat RotTurbulentCycle { get; set; }
        public FoxFloat InfluenceOfGlobal { get; set; }
    }
}
