using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppNewPlayerPadParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat LeftStickForceResponseMin { get; set; }
        public FoxFloat LeftStickForceResponseMax { get; set; }
        public FoxUInt32 LeftStickSensitivity { get; set; }
        public FoxFloat LeftStickForceWalkToRun { get; set; }
        public FoxFloat TurnGeneralInterp { get; set; }
        public FoxFloat LeftStickForceStabilityWidth { get; set; }
        public FoxFloat LeftStickDirectionStabilityAngle { get; set; }
        public FoxFloat ParaCrawlMoveFBWalkSpeed { get; set; }
        public FoxFloat ParaCrawlMoveFBRunSpeed { get; set; }
        public FoxFloat ParaCrawlMoveRLWalkSpeed { get; set; }
        public FoxFloat ManualTurnInterp_NormalStand { get; set; }
        public FoxFloat ManualTurnInterp_NormalSquat { get; set; }
        public FoxFloat ManualTurnInterp_NormalCrawl { get; set; }
        public FoxFloat ManualTurnInterp_Parallel { get; set; }
        public FoxFloat StanceChangePressTime { get; set; }
        public FoxFloat CrawlForwardMoveDir { get; set; }
        public FoxFloat CrawlTurnMaxAngle { get; set; }
        public FoxBool ControlJumpPathDir { get; set; }
        public FoxFloat Jump100to250Boundary { get; set; }
        public FoxFloat Jump250to450Boundary { get; set; }
        public FoxFloat SubjectiveWeaponPositionZ { get; set; }
        public FoxFloat SlopeMotionAngle { get; set; }
        public FoxFloat CqcHoldTriggerTime { get; set; }
        public FoxFloat CliffMaxSpeed { get; set; }
    }
}
