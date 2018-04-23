using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSupportHelicopterLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxFloat AngleBodyMax { get; set; }
        public FoxFloat AccelRateXZ { get; set; }
        public FoxFloat AccelRateY { get; set; }
        public FoxFloat AirResistance { get; set; }
        public FoxFloat FeedbackRateAngleXZ { get; set; }
        public FoxFloat FeedbackRateAngularVelocityXZ { get; set; }
        public FoxFloat FeedbackRateAngleY { get; set; }
        public FoxFloat FeedbackRateAngularVelocityY { get; set; }
        public FoxFloat LifeMax { get; set; }
        public FoxFloat SpeedToCalledPoint { get; set; }
        public FoxFloat SpeedToRendezvousPoint { get; set; }
        public FoxFloat SpeedFromRendezvousPoint { get; set; }
        public FoxFloat SpeedToDropPoint { get; set; }
        public FoxFloat SpeedForDropping { get; set; }
        public FoxFloat SpeedFromDropPoint { get; set; }
        public FoxFloat Height { get; set; }
        public FoxString MainRotorBoneName { get; set; }
        public FoxString RearRotorBoneName { get; set; }
        public FoxFloat RearRotorAngleZ { get; set; }
        public FoxEntityLink Crews { get; set; }
        public FoxString ConnectPointNames { get; set; }
    }
}
