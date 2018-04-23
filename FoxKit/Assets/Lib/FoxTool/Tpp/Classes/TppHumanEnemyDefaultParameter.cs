using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppHumanEnemyDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxString InitWeaponId { get; set; }
        public FoxFloat EyeRange { get; set; }
        public FoxFloat EyeHeadBackDistance { get; set; }
        public FoxFloat EyeHeadBackDistanceFar { get; set; }
        public FoxFloat EyeRangeDiscovery { get; set; }
        public FoxFloat EyeRangeIndis { get; set; }
        public FoxFloat EyeRangeDim { get; set; }
        public FoxFloat EyeRangeFar { get; set; }
        public FoxFloat EyeRangeNightDiscovery { get; set; }
        public FoxFloat EyeRangeNightIndis { get; set; }
        public FoxFloat EyeRangeNightDim { get; set; }
        public FoxFloat EyeRangeNightFar { get; set; }
        public FoxFloat EyeRangeCombatDiscovery { get; set; }
        public FoxFloat EyeRangeCombatIndis { get; set; }
        public FoxFloat EyeRangeCombatDim { get; set; }
        public FoxFloat EyeRangeCombatFar { get; set; }
        public FoxFloat EyeRangeDiscoveryHard { get; set; }
        public FoxFloat EyeRangeIndisHard { get; set; }
        public FoxFloat EyeRangeDimHard { get; set; }
        public FoxFloat EyeRangeFarHard { get; set; }
        public FoxFloat EyeRangeNightDiscoveryHard { get; set; }
        public FoxFloat EyeRangeNightIndisHard { get; set; }
        public FoxFloat EyeRangeNightDimHard { get; set; }
        public FoxFloat EyeRangeNightFarHard { get; set; }
        public FoxFloat EyeRangeCombatDiscoveryHard { get; set; }
        public FoxFloat EyeRangeCombatIndisHard { get; set; }
        public FoxFloat EyeRangeCombatDimHard { get; set; }
        public FoxFloat EyeRangeCombatFarHard { get; set; }
        public FoxFloat EyeRangeRateSandstorm { get; set; }
        public FoxFloat EyeRangeRateSandstormFar { get; set; }
        public FoxFloat EyeRangeRateRain { get; set; }
        public FoxFloat EyeRangeRateRainFar { get; set; }
        public FoxFloat EyeAngleLeftRightSandstorm { get; set; }
        public FoxFloat EyeAngleLeftRightRain { get; set; }
        public FoxFloat EyeAngleLeftRightFar { get; set; }
        public FoxFloat EyeAngleUpDownFar { get; set; }
        public FoxFloat EyeAngleLeftRight { get; set; }
        public FoxFloat EyeAngleUpDown { get; set; }
        public FoxFloat EyeGazeMaxRate { get; set; }
        public FoxFloat EyeGazeMaxTime { get; set; }
        public FoxFloat EyeParamUpStartHour { get; set; }
        public FoxFloat EyeParamUpEndHour { get; set; }
        public FoxFloat EyeParamDownStartHour { get; set; }
        public FoxFloat EyeParamDownEndHour { get; set; }
        public FoxFloat DiscoveryAsEnemyCamouflageValue { get; set; }
        public FoxFloat DiscoveryAsCharacterCamouflageValue { get; set; }
        public FoxFloat DiscoveryAsObjectCamouflageValue { get; set; }
        public FoxFloat IndisAsEnemyCamouflageValue { get; set; }
        public FoxFloat IndisAsCharacterCamouflageValue { get; set; }
        public FoxFloat IndisAsObjectCamouflageValue { get; set; }
        public FoxFloat DimAsEnemyCamouflageValue { get; set; }
        public FoxFloat DimAsCharacterCamouflageValue { get; set; }
        public FoxFloat DimAsObjectCamouflageValue { get; set; }
        public FoxFloat FarAsEnemyCamouflageValue { get; set; }
        public FoxFloat FarAsCharacterCamouflageValue { get; set; }
        public FoxFloat FarAsObjectCamouflageValue { get; set; }
        public FoxFloat DiscoveryAsEnemyCamouflageValueHard { get; set; }
        public FoxFloat DiscoveryAsCharacterCamouflageValueHard { get; set; }
        public FoxFloat DiscoveryAsObjectCamouflageValueHard { get; set; }
        public FoxFloat IndisAsEnemyCamouflageValueHard { get; set; }
        public FoxFloat IndisAsCharacterCamouflageValueHard { get; set; }
        public FoxFloat IndisAsObjectCamouflageValueHard { get; set; }
        public FoxFloat DimAsEnemyCamouflageValueHard { get; set; }
        public FoxFloat DimAsCharacterCamouflageValueHard { get; set; }
        public FoxFloat DimAsObjectCamouflageValueHard { get; set; }
        public FoxFloat FarAsEnemyCamouflageValueHard { get; set; }
        public FoxFloat FarAsCharacterCamouflageValueHard { get; set; }
        public FoxFloat FarAsObjectCamouflageValueHard { get; set; }
        public Dictionary<string, FoxFloat> EarRanges { get; set; }
        public Dictionary<string, FoxFloat> EarRangesHard { get; set; }
        public FoxFloat FultonRecovered1stStepHeight { get; set; }
        public FoxFloat FultonRecovered1stStepTime { get; set; }
        public FoxFloat FultonRecoveredIdleTime { get; set; }
        public FoxFloat FultonRecovered2ndStepHeight { get; set; }
        public FoxFloat FultonRecovered2ndStepTime { get; set; }
        public FoxFloat LifeRecoverTime { get; set; }
        public FoxFloat SleepRecoverTime { get; set; }
        public FoxFloat FaintRecoverTime { get; set; }
        public FoxFloat SleepRecoverTimeHard { get; set; }
        public FoxFloat FaintRecoverTimeHard { get; set; }
        public FoxInt32 FaintRecoverMin { get; set; }
        public FoxFloat LifeDamageTime { get; set; }
        public FoxFloat UnrealMoveSpeeds { get; set; }
        public FoxFloat HoldUpDistance { get; set; }
        public FoxFloat CallFarClearingDistance { get; set; }
        public FoxFloat ClearingCharaRange { get; set; }
        public FoxFloat MortarSearchRange { get; set; }
        public FoxFloat SniperRange { get; set; }
        public FoxFloat PhaseNeutralToSneak1Radius { get; set; }
        public FoxFloat PhaseSneak1ToSneak2Radius { get; set; }
        public FoxFloat ViewAngleHorizontal { get; set; }
        public FoxFloat ViewAngleVertical { get; set; }
        public FoxFloat FireBlurDistance { get; set; }
        public FoxFloat FireBlurWidth { get; set; }
        public FoxFloat FireBlurDistanceHard { get; set; }
        public FoxFloat FireBlurWidthHard { get; set; }
        public FoxUInt32 SmokeDamageMaxLoop { get; set; }
        public FoxUInt32 SquatSmokeDamageProbability { get; set; }
        public FoxFloat BloodStainRadiusBullet { get; set; }
        public FoxFloat BloodStainRadiusLargeCaliberBullet { get; set; }
        public FoxFloat BloodStainRadiusExplosion { get; set; }
        public FoxFloat SootRadiusExplosion { get; set; }
        public FoxFloat SearchLightSearchRadius { get; set; }
        public FoxFloat SearchLightSearchTime { get; set; }
        public FoxFloat SearchLightSearchSpeed { get; set; }
        public FoxVector3 OffsetHomingObject { get; set; }
        public FoxFloat InterrogationMarkAroudEnemyRadius { get; set; }
        public FoxUInt32 BaseThreatLevelAlertLost { get; set; }
        public FoxUInt32 BaseThreatLevelEvasion { get; set; }
        public FoxUInt32 BaseThreatLevelCaution { get; set; }
        public FoxUInt32 BaseThreatLevelSneak { get; set; }
        public FoxFloat ThreatLevelDownWaitTime { get; set; }
        public FoxUInt32 ThreatLevelDownValue { get; set; }
        public FoxFloat GroupVehicleSearchVehicleRange { get; set; }
    }
}
