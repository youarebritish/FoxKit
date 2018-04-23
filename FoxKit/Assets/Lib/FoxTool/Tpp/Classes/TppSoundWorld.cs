using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSoundWorld : Data
    {
        // Static properties
        public FoxUInt32 UpdateSeconds { get; set; }
        public FoxFloat StartMorning { get; set; }
        public FoxFloat MidMorning { get; set; }
        public FoxFloat EndMorning { get; set; }
        public FoxFloat StartEvening { get; set; }
        public FoxFloat MidEvening { get; set; }
        public FoxFloat EndEvening { get; set; }
        public FoxString SituationEvent { get; set; }
        public FoxString ClockRtpc { get; set; }
        public FoxString WindVelocityRtpc { get; set; }
        public FoxString WindDirectionRtpc { get; set; }
        public FoxString RainRtpc { get; set; }
        public FoxString HeightRtpc { get; set; }
        public List<FoxEntityLink> AmbientParameter { get; set; }
        public FoxString CategoryFpvStateGroup { get; set; }
        public FoxString CategoryFpvStateValue { get; set; }
        public FoxString DashStartEventName { get; set; }
        public FoxString DashFinishEventName { get; set; }
        public FoxFloat BlockedObstruction { get; set; }
        public FoxFloat BlockedOcclusion { get; set; }
        public FoxFloat UnlinkedObstruction { get; set; }
        public FoxFloat UnlinkedOcclusion { get; set; }
        public FoxFloat InterferenceSlope { get; set; }
    }
}
