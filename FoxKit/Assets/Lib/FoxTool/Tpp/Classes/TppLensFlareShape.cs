using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLensFlareShape : TransformData
    {
        // Static properties
        public FoxEntityLink Material { get; set; }
        public FoxFloat Width { get; set; }
        public FoxFloat Height { get; set; }
        public FoxColor BaseColor { get; set; }
        public FoxInt32 OffsetType { get; set; }
        public FoxFloat OffsetScale { get; set; }
        public FoxFloat BaseOffsetX { get; set; }
        public FoxFloat BaseOffsetY { get; set; }
        public FoxInt32 RotateType { get; set; }
        public FoxFloat RotateScale { get; set; }
        public FoxFloat BaseRotate { get; set; }
        public FoxEntityLink ScaleFieldX { get; set; }
        public FoxEntityLink ScaleFieldY { get; set; }
        public FoxBool ScaleFieldPickSunPositionFlag { get; set; }
        public FoxEntityLink AlphaField { get; set; }
        public FoxBool AlphaFieldPickSunPositionFlag { get; set; }
        public FoxFloat ShieldFadeOutTime { get; set; }
        public FoxFloat ShieldFadeInTime { get; set; }
        public FoxEntityLink AngleScaleGraphX { get; set; }
        public FoxEntityLink AngleScaleGraphY { get; set; }
        public FoxEntityLink AngleAlphaGraph { get; set; }
        public FoxInt32 DistanceScaling { get; set; }
        public FoxFloat LimitDistance { get; set; }
        public FoxBool NotDrawMultiple { get; set; }
        public FoxString SeName { get; set; }
        public FoxFloat SeCallThreshold { get; set; }
        public FoxBool LockToWorldY { get; set; }
    }
}
