using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSkyClouds3Param
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxBool Enable { get; set; }
        public FoxBool FollowCamera { get; set; }
        public FoxColor Color { get; set; }
        public FoxFloat LuminanceScale { get; set; }
        public FoxFloat Bottom { get; set; }
        public FoxFloat Radius { get; set; }
        public FoxFloat Height { get; set; }
        public FoxFloat DomeLength { get; set; }
        public FoxFloat DomeStreach { get; set; }
        public FoxFloat DomeWindInfluence { get; set; }
        public FoxFloat MidCylinderPos { get; set; }
        public FoxFloat MidCylinderWidth { get; set; }
        public FoxFloat MidCylinderStreach { get; set; }
        public FoxFloat MidCylinderScrSpeed { get; set; }
        public FoxFloat LowCylinderIntrusion { get; set; }
        public FoxFloat LowCylinderStreach { get; set; }
        public FoxFloat LowCylinderScrSpeed { get; set; }
        public FoxUInt32 CylinderTexRepeat { get; set; }
        public FoxPath DomeTexture { get; set; }
        public FoxPath MidCylinderTexture { get; set; }
        public FoxPath LowCylinderTexture { get; set; }
        public FoxInt32 ColorSpace { get; set; }
        public FoxInt32 TextureColorHandling { get; set; }
    }
}
