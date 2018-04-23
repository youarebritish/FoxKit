using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class NavxNavigableParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString Name { get; set; }
        public FoxBool IsDefault { get; set; }
        public FoxFloat Radius { get; set; }
        public FoxFloat SimplificationThreshold { get; set; }
        public FoxFloat Height { get; set; }
        public FoxFloat MaxClimbableAngle { get; set; }
        public FoxFloat MaxStepSize { get; set; }
    }
}
