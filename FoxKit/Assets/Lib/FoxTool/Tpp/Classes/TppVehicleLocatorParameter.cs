using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppVehicleLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
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
        public FoxString PatrolRouteId { get; set; }
        public FoxUInt32 PatrolRoutePoint { get; set; }
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
        public FoxString CautionRouteId { get; set; }
        public FoxUInt32 CautionRoutePoint { get; set; }
        public FoxEntityLink Squad { get; set; }
        public FoxBool IsReadyStatus { get; set; }
        public FoxPath DamageSet { get; set; }
        public FoxUInt32 VehicleType { get; set; }
        public FoxBool IsFromMotherBase { get; set; }
        public FoxString NavAttributeName { get; set; }
        public FoxString FormVariationKeyName { get; set; }
        public FoxUInt32 FormVariationRandomSeed { get; set; }
    }
}
