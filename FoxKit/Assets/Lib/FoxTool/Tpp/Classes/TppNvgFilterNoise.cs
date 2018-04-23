using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppNvgFilterNoise : Data
    {
        // Static properties
        public FoxBool Visibility { get; set; }
        public FoxFloat Scale { get; set; }
        public FoxFloat Offset { get; set; }
        public FoxFloat CutScale { get; set; }
        public FoxFloat CutOffset { get; set; }
        public FoxBool IsForceVisible { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat RadialSlope { get; set; }
        public FoxFloat RadialShift { get; set; }
    }
}
