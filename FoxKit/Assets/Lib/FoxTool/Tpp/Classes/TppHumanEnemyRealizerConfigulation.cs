using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppHumanEnemyRealizerConfigulation : Data
    {
        // Static properties
        public FoxFloat RealizeRange { get; set; }
        public FoxFloat UnrealizeRange { get; set; }
        public FoxUInt8 PriorityUnitSize { get; set; }
        public FoxUInt8 FramePerCycleMin { get; set; }
        public FoxUInt8 FramePerCycleMax { get; set; }
    }
}
