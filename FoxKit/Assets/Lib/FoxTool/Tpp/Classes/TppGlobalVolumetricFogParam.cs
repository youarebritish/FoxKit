using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppGlobalVolumetricFogParam
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxBool Enable { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat Luminance { get; set; }
        public FoxColor Albedo { get; set; }
        public FoxFloat Density { get; set; }
        public FoxFloat NearDistance { get; set; }
        public FoxFloat Far { get; set; }
        public FoxFloat Height { get; set; }
        public FoxFloat Bottom { get; set; }
        public FoxFloat Falloff { get; set; }
        public FoxFloat InfluenceOfAtmosphere { get; set; }
        public FoxFloat InfluenceOfLightning { get; set; }
        public FoxFloat PseudoAbsorption { get; set; }
        public FoxFloat PseudoDiffusion { get; set; }
        public FoxFloat Turbidity { get; set; }
        public FoxFloat AerialPerspective { get; set; }
        public FoxColor AerialPersColor { get; set; }
    }
}
