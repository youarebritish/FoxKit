using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhMultiHingeConstraintParam
    {
        // Static properties
        public FoxVector3 DefaultPosition { get; set; }
        public FoxVector3 Axis { get; set; }
        public FoxBool LimitedFlag { get; set; }
        public FoxBool IsPoweredFlag { get; set; }
        public FoxFloat LimitHi { get; set; }
        public FoxFloat LimitLo { get; set; }
        public FoxInt32 ControlType { get; set; }
        public FoxFloat VelocityMax { get; set; }
        public FoxFloat TorqueMax { get; set; }
        public FoxFloat TargetTheta { get; set; }
        public FoxFloat TargetVelocity { get; set; }
        public FoxFloat VelocityRate { get; set; }
    }
}
