using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhRigidBodyParam
    {
        // Static properties
        public FoxVector3 DefaultPosition { get; set; }
        public FoxQuat DefaultRotation { get; set; }
        public FoxFloat Mass { get; set; }
        public FoxFloat Friction { get; set; }
        public FoxFloat Restitution { get; set; }
        public FoxFloat MaxLinearVelocity { get; set; }
        public FoxFloat MaxAngularVelocity { get; set; }
        public FoxFloat LinearVelocityDamp { get; set; }
        public FoxFloat AngularVelocityDamp { get; set; }
        public FoxFloat PermittedDepth { get; set; }
        public FoxBool SleepEnable { get; set; }
        public FoxFloat SleepLinearVelocityTh { get; set; }
        public FoxFloat SleepAngularVelocityTh { get; set; }
        public FoxFloat SleepTimeTh { get; set; }
        public FoxUInt16 CollisionGroup { get; set; }
        public FoxUInt16 CollisionType { get; set; }
        public FoxUInt32 CollisionId { get; set; }
        public FoxVector3 CenterOfMassOffset { get; set; }
        public FoxInt32 MotionType { get; set; }
        public FoxString Material { get; set; }
        public FoxBool IsNoGravity { get; set; }
    }
}
