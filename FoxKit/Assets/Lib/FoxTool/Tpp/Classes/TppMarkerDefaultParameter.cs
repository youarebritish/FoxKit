using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppMarkerDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat SignFadeInTime { get; set; }
        public FoxFloat SignFadeOutTime { get; set; }
        public FoxFloat NearEnemyAutoTagRange { get; set; }
        public FoxFloat NearEnemyAutoTagTime { get; set; }
        public FoxFloat ThreatIndicatorCheckInterval { get; set; }
        public FoxFloat ThreatIndicatorCheckLength { get; set; }
        public FoxUInt32 ThreatIndicatorCheckNum { get; set; }
        public FoxFloat ThreatIndicatorSightInCheckInterval { get; set; }
        public FoxFloat DynamicSensorCheckLength { get; set; }
        public FoxFloat DynamicSensorInCheckInterval { get; set; }
        public FoxFloat DynamicSensorNearLength { get; set; }
        public FoxFloat ActiveSonerInCheckInterval { get; set; }
        public FoxFloat ActiveSonerLife { get; set; }
        public FoxFloat AutoMarkingLength { get; set; }
        public FoxBool CheckPlayerMotionForSoner { get; set; }
    }
}
