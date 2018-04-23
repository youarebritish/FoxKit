using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhxWheelConstraintParam
    {
        // Static properties
        public FoxVector3 DefaultPosition { get; set; }
        public FoxQuat DefaultRotation { get; set; }
        public FoxVector3 PositionL { get; set; }
        public FoxVector3 FrontL { get; set; }
        public FoxVector3 UpL { get; set; }
        public FoxVector3 WheelPositionOffset { get; set; }
        public FoxFloat Radius { get; set; }
        public FoxFloat SuspensionLength { get; set; }
        public FoxFloat MaxSuspensionForce { get; set; }
        public FoxFloat DampingFactorElong { get; set; }
        public FoxFloat DampingFactorCompress { get; set; }
        public FoxFloat Friction { get; set; }
        public FoxFloat Restitution { get; set; }
        public FoxFloat Inertia { get; set; }
    }
}
