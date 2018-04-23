using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SoundPhysicsParameter : Data
    {
        // Static properties
        public FoxString HitEvent { get; set; }
        public FoxString RollStartEvent { get; set; }
        public FoxString RollEndEvent { get; set; }
        public FoxString HitRtpcName { get; set; }
        public FoxString RollRtpcName { get; set; }
        public FoxString SwitchName { get; set; }
        public FoxFloat HitLowerPower { get; set; }
        public FoxFloat HitUpperPower { get; set; }
        public FoxFloat HitIntervalSeconds { get; set; }
        public FoxFloat HitLowerRtpc { get; set; }
        public FoxFloat HitUpperRtpc { get; set; }
        public FoxFloat RollLowerPower { get; set; }
        public FoxFloat RollUpperPower { get; set; }
        public FoxFloat RollStartSeconds { get; set; }
        public FoxFloat RollEndSeconds { get; set; }
        public FoxFloat RollLowerRtpc { get; set; }
        public FoxFloat RollUpperRtpc { get; set; }
    }
}
