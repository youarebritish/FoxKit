using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppHumanEnemyLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString PatrolRouteId { get; set; }
        public FoxUInt32 PatrolRoutePoint { get; set; }
        public FoxString CautionRouteId { get; set; }
        public FoxUInt32 CautionRoutePoint { get; set; }
        public FoxPath DamageSet { get; set; }
        public FoxEntityLink Respawner { get; set; }
        public FoxEntityLink Squad { get; set; }
        public FoxUInt32 LifeMax { get; set; }
        public FoxString InitWeaponId { get; set; }
        public FoxString VoiceType { get; set; }
        public FoxBool EnableSpeak { get; set; }
        public FoxBool EnableHeadMark { get; set; }
        public FoxBool IsUIHeadMark { get; set; }
        public FoxEntityLink GuardPoint { get; set; }
        public FoxBool IsReadyStatus { get; set; }
        public FoxPath PartsPath { get; set; }
        public FoxUInt32 FaintMax { get; set; }
        public FoxUInt32 SleepMax { get; set; }
        public FoxInt32 LifeInitial { get; set; }
        public FoxInt32 FaintInitial { get; set; }
        public FoxInt32 SleepInitial { get; set; }
        public FoxUInt32 AnesthMax { get; set; }
        public FoxUInt32 LifeLArmMax { get; set; }
        public FoxUInt32 LifeRArmMax { get; set; }
        public FoxUInt32 LifeLLegMax { get; set; }
        public FoxUInt32 LifeRLegMax { get; set; }
        public FoxString RecordBankKeyName { get; set; }
        public FoxString FormVariationKeyName { get; set; }
        public FoxUInt32 FormVariationRandomSeed { get; set; }
        public FoxUInt32 MotherBaseKeyIndex { get; set; }
        public FoxString EnemyType { get; set; }
        public FoxBool IsThrowGrenade { get; set; }
        public FoxFloat ProbThrowGrenade { get; set; }
        public FoxBool IsUseThreatMove { get; set; }
        public FoxBool IsUseEyeParamFromLocator { get; set; }
        public FoxFloat EyeRangeDiscovery { get; set; }
        public FoxFloat EyeRangeIndis { get; set; }
        public FoxFloat EyeRangeDim { get; set; }
        public FoxFloat EyeRangeFar { get; set; }
        public FoxBool IsEnableEye { get; set; }
        public FoxBool IsEnableEar { get; set; }
        public FoxBool IsZombie { get; set; }
        public FoxBool IsAlien { get; set; }
        public FoxInt32 VehicleRideRole { get; set; }
        public FoxString StanceTypesOfAssaultRifle { get; set; }
        public FoxString StanceTypesOfSubmachineGun { get; set; }
        public FoxEntityLink VehicleLocator { get; set; }
    }
}
