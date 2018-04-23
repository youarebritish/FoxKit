using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppCommandPostLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString CpType { get; set; }
        public FoxString HqCharacterId { get; set; }
        public FoxFloat SquadDivisionRadius { get; set; }
        public FoxFloat SpreadSpeed { get; set; }
        public FoxFloat SpreadMaxRadius { get; set; }
        public FoxFloat RadioClearHearingRange { get; set; }
        public FoxFloat RadioNoisyHearingRange { get; set; }
        public FoxFloat PhaseAlertMaxLevel { get; set; }
        public FoxFloat PhaseEvasionMaxLevel { get; set; }
        public FoxFloat PhaseCautionMaxLevel { get; set; }
        public FoxString MinPhaseName { get; set; }
        public FoxString CpName { get; set; }
        public FoxString VoiceType { get; set; }
        public FoxEntityLink QrfSquads { get; set; }
        public FoxEntityLink ReserveSquads { get; set; }
        public FoxEntityLink HelpSquads { get; set; }
        public FoxEntityLink FixedSquads { get; set; }
        public FoxEntityLink VehicleSquads { get; set; }
        public FoxString CommonRouteIds { get; set; }
        public FoxEntityLink CombatLocatorSet { get; set; }
        public FoxEntityLink CautionLocatorSet { get; set; }
        public List<FoxEntityLink> GuardTargets { get; set; }
        public FoxEntityLink CombatIndexAreas { get; set; }
        public FoxFloat AttackAssignPercent { get; set; }
        public FoxFloat GuardAssignPercent { get; set; }
        public FoxUInt32 AttackAssignMinimum { get; set; }
        public FoxUInt32 GuardAssignMinimum { get; set; }
        public FoxUInt32 WaitAssignMinimum { get; set; }
        public FoxBool EnableCombatStraightLineCheck { get; set; }
        public FoxFloat CombatStraightLineCheckLimitAngle { get; set; }
        public FoxFloat CombatChangeOffset { get; set; }
        public FoxFloat CombatStraightCheckOutAngleCost { get; set; }
        public List<FoxEntityLink> Enemies { get; set; }
        public List<FoxEntityLink> RouteSets { get; set; }
        public FoxEntityLink QrfEnemies { get; set; }
        public FoxEntityLink ReserveEnemies { get; set; }
        public FoxEntityLink HelpEnemies { get; set; }
        public FoxEntityLink FixedEnemies { get; set; }
        public FoxEntityLink VehicleEnemies { get; set; }
        public FoxPath CpUpdateScript { get; set; }
        public FoxFloat TerritoryRadius { get; set; }
        public FoxUInt16 AlertLostSec { get; set; }
        public FoxFloat CallHelpPercent { get; set; }
        public FoxUInt16 CallHelpIntervalSec { get; set; }
        public FoxString RadioCharacterId { get; set; }
        public FoxString AntennaCharacterIds { get; set; }
        public FoxEntityLink Ops { get; set; }
        public FoxEntityLink SpawnPoints { get; set; }
        public FoxUInt16 OpReinforceLimit { get; set; }
        public FoxUInt8 OpReinforceUnit { get; set; }
        public FoxEntityLink Vehicles { get; set; }
        public FoxEntityLink GroupVehicleRouteInfos { get; set; }
        public FoxBool PhantomReinforce { get; set; }
        public FoxEntityLink SpecialCharaInterrogationData { get; set; }
        public FoxEntityLink CommonInterrogationData { get; set; }
        public FoxEntityLink EnemyConversationData { get; set; }
    }
}
