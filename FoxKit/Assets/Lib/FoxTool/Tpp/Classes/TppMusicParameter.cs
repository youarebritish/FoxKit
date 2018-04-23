using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppMusicParameter : Data
    {
        // Static properties
        public FoxString Tag { get; set; }
        public FoxString PlayEvent { get; set; }
        public FoxString DaySwitchEvent { get; set; }
        public FoxString NightSwitchEvent { get; set; }
        public FoxString DangerEasySwitchEvent { get; set; }
        public FoxString DangerBattleSwitchEvent { get; set; }
        public FoxString DangerHardSwitchEvent { get; set; }
        public FoxString DangerEasyLostSwitchEvent { get; set; }
        public FoxString DangerBattleLostSwitchEvent { get; set; }
        public FoxString DangerHardLostSwitchEvent { get; set; }
        public FoxString DangerEvasionSwitchEvent { get; set; }
        public FoxString DangerStrongSwitchEvent { get; set; }
        public FoxString SafetyNeutralToSneakSwitchEvent { get; set; }
        public FoxString SafetySneakSwitchEvent { get; set; }
        public FoxString SafetyCautionSwitchEvent { get; set; }
        public FoxString SafetyNoticeSwitchEvent { get; set; }
        public FoxString SafetyCautionNoticeSwitchEvent { get; set; }
        public FoxString SafetyAlertToCautionSwitchEvent { get; set; }
        public FoxString NeutralSwitchEvent { get; set; }
    }
}
