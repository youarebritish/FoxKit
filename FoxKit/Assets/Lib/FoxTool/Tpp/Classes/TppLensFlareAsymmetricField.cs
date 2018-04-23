using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLensFlareAsymmetricField : Data
    {
        // Static properties
        public FoxInt32 ShapeType { get; set; }
        public FoxInt32 InterpType { get; set; }
        public FoxColor DebugDrawColor { get; set; }
        public FoxFloat InnerScale { get; set; }
        public FoxFloat CenterScale { get; set; }
        public FoxFloat OuterScale { get; set; }
        public FoxFloat InnerValue { get; set; }
        public FoxFloat CenterValue { get; set; }
        public FoxFloat OuterValue { get; set; }
        public FoxBool Reverse { get; set; }
        public FoxFloat VerticalInnerScale { get; set; }
        public FoxFloat VerticalCenterScale { get; set; }
        public FoxFloat VerticalOuterScale { get; set; }
        public FoxFloat VerticalInnerValue { get; set; }
        public FoxFloat VerticalCenterValue { get; set; }
        public FoxFloat VerticalOuterValue { get; set; }
    }
}
