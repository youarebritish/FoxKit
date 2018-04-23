using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhMultiShoulderConstraintParam
    {
        // Static properties
        public FoxVector3 DefaultPosition { get; set; }
        public FoxVector3 RefVec0 { get; set; }
        public FoxVector3 RefVec1 { get; set; }
        public FoxFloat RefLimit0 { get; set; }
        public FoxFloat RefLimit1 { get; set; }
        public FoxFloat VelocityMax { get; set; }
        public FoxFloat TorqueMax { get; set; }
        public FoxFloat VelocityRate { get; set; }
        public FoxBool IsPoweredFlag { get; set; }
    }
}
