using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppStrykerDefaultParameter2
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat GearRatioFactor { get; set; }
        public FoxFloat EngineBreakFactor { get; set; }
        public FoxFloat SteeringRotFactor { get; set; }
        public FoxFloat SteeringRotSpeedFactor { get; set; }
        public FoxFloat SteeringRestSpeedFactor { get; set; }
        public FoxFloat MaxSpeed { get; set; }
        public FoxFloat DifferentialFactor { get; set; }
        public FoxBool EnableStabilizer { get; set; }
        public FoxFloat StabilizePower { get; set; }
        public FoxFloat StabilizeStart { get; set; }
        public FoxFloat StabilizeLimit { get; set; }
        public FoxFloat EngineRpmLimit { get; set; }
        public FoxFloat EngineRpmToShiftUp { get; set; }
        public FoxFloat EngineRpmToShiftDown { get; set; }
        public FoxFloat FinalGearRatio { get; set; }
        public List<FoxFloat> TransmissionGearRatios { get; set; }
        public FoxFloat YAxisRotationReductionRate { get; set; }
        public FoxFloat XAxisSlipReductionRate { get; set; }
        public FoxFloat YAxisRotationReduceThreshold { get; set; }
        public FoxFloat XAxisSlipReductionThreshold { get; set; }
        public FoxBool EnableXAxisStabilizer { get; set; }
        public FoxFloat XAxisStabilizePower { get; set; }
        public FoxFloat XAxisStabilizeStart { get; set; }
        public FoxFloat XAxisStabilizeLimit { get; set; }
        public FoxBool EnableXAxisxAxisAngularVelocityDamp { get; set; }
        public FoxBool EnableZAxisxAxisAngularVelocityDamp { get; set; }
        public FoxFloat XAxisAngularVelocityDampPower { get; set; }
        public FoxFloat ZAxisAngularVelocityDampPower { get; set; }
        public FoxBool EnableSteeringWeightBySpeed { get; set; }
        public FoxFloat SteeringWeightLowLimitSpeed { get; set; }
        public FoxFloat SteeringRotSpeedFactorByMinSpeed { get; set; }
        public FoxFloat SteeringWeightHighLimitSpeed { get; set; }
        public FoxFloat SteeringRotSpeedFactorByMaxSpeed { get; set; }
        public FoxBool EnableSteeringRangeBySpeed { get; set; }
        public FoxFloat SteeringRangeLowLimitSpeed { get; set; }
        public FoxFloat SteeringRotFactorByMinSpeed { get; set; }
        public FoxFloat SteeringRangeHighLimitSpeed { get; set; }
        public FoxFloat SteeringRotFactorByMaxSpeed { get; set; }
        public FoxBool EnableESC { get; set; }
        public FoxFloat EscBrakeMaxAt60 { get; set; }
        public FoxFloat EscBrakeRateAt60 { get; set; }
        public FoxFloat MaxSpeedReverse { get; set; }
        public FoxInt32 MaxBodyLifeValue { get; set; }
        public FoxInt32 MaxWheelLifeValue { get; set; }
        public FoxFloat MinFallDamageRate { get; set; }
        public FoxFloat MaxFallDamageRate { get; set; }
        public FoxFloat MinCrashDamageRate { get; set; }
        public FoxFloat MaxCrashDamageRate { get; set; }
        public FoxFloat HeadLightRange { get; set; }
        public FoxFloat HeadLightAngle { get; set; }
        public FoxFloat BlastReactionFactor { get; set; }
        public List<FoxString> AttachPointNamesAround { get; set; }
        public FoxVector3 BasePosOffsetAround { get; set; }
        public FoxString AttachPointNamesAroundSub { get; set; }
        public FoxVector3 BasePosOffsetAroundSub { get; set; }
        public FoxString AttachPointNamesTps { get; set; }
        public FoxVector3 BasePosOffsetTps { get; set; }
        public FoxString AttachPointNamesTpsSub { get; set; }
        public FoxVector3 BasePosOffsetTpsSub { get; set; }
        public FoxString AttachPointNamesSubjective { get; set; }
        public FoxVector3 BasePosOffsetSubjective { get; set; }
        public FoxString AttachPointNamesSubjectiveSub { get; set; }
        public FoxVector3 BasePosOffsetSubjectiveSub { get; set; }
        public Dictionary<string, FoxInt32> LifeValues { get; set; }
        public FoxString FultonConnectPointNames { get; set; }
        public FoxFloat TurretRotSpeedFactor { get; set; }
        public FoxFloat TurretRotLimitX { get; set; }
        public FoxFloat TurretRotMinLimitX { get; set; }
        public FoxFloat TurretRotLimitY { get; set; }
        public FoxFloat BodyAttackSpeed { get; set; }
        public FoxFloat CannonFireInterval { get; set; }
        public FoxFloat CannonFireForce { get; set; }
        public FoxInt32 TurretLifeMax { get; set; }
        public FoxFloat TurretRotSpeedFactorForSubjectiveCamera { get; set; }
    }
}
