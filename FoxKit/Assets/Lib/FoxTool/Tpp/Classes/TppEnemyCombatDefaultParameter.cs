using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppEnemyCombatDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxBool IsThrowGrenade { get; set; }
        public FoxFloat ProbThrowGrenade { get; set; }
        public FoxFloat DictateGrenadeWaitMin { get; set; }
        public FoxFloat ProbDictateGrenadeMin { get; set; }
        public FoxFloat DictateGrenadeWaitMax { get; set; }
        public FoxFloat CheckGrenadeWaitTime { get; set; }
        public FoxFloat EnableThrowGrenadeLength { get; set; }
        public FoxBool IsUseThreatMove { get; set; }
        public FoxFloat ThreatRizeTime { get; set; }
        public FoxFloat ThreatDownTime { get; set; }
        public FoxBool IsUseAroundBhdShift { get; set; }
        public FoxFloat AroundBhdShiftStartWait { get; set; }
        public FoxFloat TurretCheckIntervalOnRelease { get; set; }
        public FoxFloat TurretFireDurationMin { get; set; }
        public FoxFloat TurretFireDurationMax { get; set; }
        public FoxFloat TurretFireStopDurationMin { get; set; }
        public FoxFloat TurretFireStopDurationMax { get; set; }
        public FoxFloat TurretFireSpreadDist { get; set; }
        public FoxFloat TurretFireSpreadWidth { get; set; }
        public FoxFloat TurretRotationRate { get; set; }
        public FoxFloat MortarSearchRadius { get; set; }
        public FoxFloat MortarFiresetCooldownNoonMin { get; set; }
        public FoxFloat MortarFiresetCooldownNoonMax { get; set; }
        public FoxFloat MortarFireIntervalNoonMin { get; set; }
        public FoxFloat MortarFireIntervalNoonMax { get; set; }
        public FoxUInt32 MortarFireCountNoon { get; set; }
        public FoxFloat MortarFireSpreadNoon { get; set; }
        public FoxFloat MortarFireMinDistFromFrontNoon { get; set; }
        public FoxFloat MortarFiresetCooldownNightMin { get; set; }
        public FoxFloat MortarFiresetCooldownNightMax { get; set; }
        public FoxFloat MortarFireIntervalNightMin { get; set; }
        public FoxFloat MortarFireIntervalNightMax { get; set; }
        public FoxUInt32 MortarFireCountNight { get; set; }
        public FoxFloat ImmobileCheckRadius { get; set; }
        public FoxFloat ExecBehindAroundDistanceMax { get; set; }
        public FoxFloat ExecBehindAroundImmobileTime { get; set; }
        public FoxFloat BehindAroundSurroundRadius { get; set; }
        public FoxFloat BehindAroundBaseAngleDegrees { get; set; }
        public FoxFloat BehindAroundAngleRandomMax { get; set; }
        public FoxFloat AimOffsetLengthPerLevel { get; set; }
        public FoxUInt32 AimAccuracyLevelMax { get; set; }
        public FoxUInt32 AimAccuracyLevelMaxOnReset { get; set; }
        public FoxUInt32 AimOffsetRotXMin { get; set; }
        public FoxUInt32 AimOffsetRotXMax { get; set; }
        public FoxUInt32 AimOffsetRotZMin { get; set; }
        public FoxUInt32 AimOffsetRotZMax { get; set; }
        public FoxFloat AimWorstAccuracyDistance { get; set; }
        public FoxFloat AimBestAccuracyDistance { get; set; }
        public FoxFloat AimOffsetLengthPerLevelHard { get; set; }
        public FoxUInt32 AimAccuracyLevelMaxHard { get; set; }
        public FoxUInt32 AimAccuracyLevelMaxOnResetHard { get; set; }
        public FoxUInt32 AimOffsetRotXMinHard { get; set; }
        public FoxUInt32 AimOffsetRotXMaxHard { get; set; }
        public FoxUInt32 AimOffsetRotZMinHard { get; set; }
        public FoxUInt32 AimOffsetRotZMaxHard { get; set; }
        public FoxFloat AimWorstAccuracyDistanceHard { get; set; }
        public FoxFloat AimBestAccuracyDistanceHard { get; set; }
        public FoxFloat CoverDisableRadiusOnEnemyDead { get; set; }
        public FoxFloat CoverDisableTimeOnEnemyDead { get; set; }
        public FoxFloat CoverPeepingProbability { get; set; }
        public FoxFloat EvacuationIntervalDistance { get; set; }
        public FoxFloat AntiAircraftNoticeRange { get; set; }
        public FoxFloat AntiAircraftGunSearchRange { get; set; }
    }
}
