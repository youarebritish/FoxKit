using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLightProbe : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxBool Enable { get; set; }
        public FoxEntityLink LightArea { get; set; }
        public FoxEntityLink InnerArea { get; set; }
        public FoxEntityLink ShCoefficientsData { get; set; }
        public FoxEntityLink OnLights { get; set; }
        public FoxEntityLink OffLights { get; set; }
        public FoxFloat LuminanceScale { get; set; }
        public FoxFloat InnerScaleXPositive { get; set; }
        public FoxFloat InnerScaleXNegative { get; set; }
        public FoxFloat InnerScaleYPositive { get; set; }
        public FoxFloat InnerScaleYNegative { get; set; }
        public FoxFloat InnerScaleZPositive { get; set; }
        public FoxFloat InnerScaleZNegative { get; set; }
        public FoxInt32 Priority { get; set; }
        public FoxInt32 DebugMode { get; set; }
        public FoxInt32 DrawRejectionLevel { get; set; }
        public FoxFloat Exposure { get; set; }
        public FoxUInt32 LocalFlags { get; set; }
    }
}
