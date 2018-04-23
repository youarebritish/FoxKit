using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SpotLight : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxEntityLink LightArea { get; set; }
        public FoxBool Enable { get; set; }
        public FoxEntityLink LookAtPoint { get; set; }
        public FoxVector3 ReachPoint { get; set; }
        public FoxFloat InnerRange { get; set; }
        public FoxFloat OuterRange { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat Temperature { get; set; }
        public FoxFloat ColorDeflection { get; set; }
        public FoxFloat Lumen { get; set; }
        public FoxFloat LightSize { get; set; }
        public FoxFloat UmbraAngle { get; set; }
        public FoxFloat PenumbraAngle { get; set; }
        public FoxFloat AttenuationExponent { get; set; }
        public FoxFloat ShadowUmbraAngle { get; set; }
        public FoxFloat ShadowPenumbraAngle { get; set; }
        public FoxFloat ShadowAttenuationExponent { get; set; }
        public FoxFloat Dimmer { get; set; }
        public FoxFloat ShadowBias { get; set; }
        public FoxFloat ViewBias { get; set; }
        public FoxFloat PowerScale { get; set; }
        public FoxFloat LodFarSize { get; set; }
        public FoxFloat LodNearSize { get; set; }
        public FoxFloat LodShadowDrawRate { get; set; }
        public FoxInt32 LodRadiusLevel { get; set; }
        public FoxUInt8 LodFadeType { get; set; }
        public FoxBool CastShadow { get; set; }
        public FoxBool IsBounced { get; set; }
        public FoxBool ShowObject { get; set; }
        public FoxBool ShowRange { get; set; }
        public FoxBool IsDebugLightVolumeBound { get; set; }
        public FoxBool UseAutoDimmer { get; set; }
        public FoxBool HasSpecular { get; set; }
    }
}
