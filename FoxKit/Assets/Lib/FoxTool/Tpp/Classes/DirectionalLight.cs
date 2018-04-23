using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DirectionalLight : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxBool Enable { get; set; }
        public FoxVector3 Direction { get; set; }
        public FoxFloat ShadowRange { get; set; }
        public FoxFloat ShadowRangeExtra { get; set; }
        public FoxFloat HiResShadowRange { get; set; }
        public FoxFloat ShadowProjectionRange { get; set; }
        public FoxFloat ShadowFadeRange { get; set; }
        public FoxFloat SelfShadowBias { get; set; }
        public FoxBool IsCascadeBlend { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat Temperature { get; set; }
        public FoxFloat ColorDeflection { get; set; }
        public FoxFloat Lux { get; set; }
        public FoxFloat LightSize { get; set; }
        public FoxFloat ShadowMaskSpecular { get; set; }
        public FoxFloat ShadowOffsetStartAngle { get; set; }
        public FoxFloat ShadowOffsetEndAngle { get; set; }
        public FoxBool CastShadow { get; set; }
        public FoxBool IsBounced { get; set; }
        public FoxBool ShowObject { get; set; }
    }
}
