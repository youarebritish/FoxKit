using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppRainFilter : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxFloat StartFadeInDistance { get; set; }
        public FoxFloat EndFadeInDistance { get; set; }
        public FoxFloat StartFadeOutDistance { get; set; }
        public FoxFloat EndFadeOutDistance { get; set; }
        public FoxFloat AlbedoExtinctionRatio { get; set; }
        public FoxFloat RoughnessExtinctionCoefficient { get; set; }
        public FoxFloat RoughnessEffectiveThreshold { get; set; }
        public FoxFloat LABDiffuseScale { get; set; }
        public FoxFloat LABDiffuseAdd { get; set; }
        public FoxFloat FloorTexScale { get; set; }
        public FoxFloat WallTexScale0 { get; set; }
        public FoxFloat WallTexScale1 { get; set; }
        public FoxVector4 WallTexSpeed { get; set; }
        public FoxFloat MaskTexScale0 { get; set; }
        public FoxFloat MaskTexScale1 { get; set; }
        public FoxVector4 MaskTexSpeed { get; set; }
        public FoxColor RainColor { get; set; }
        public FoxFloat WindScale { get; set; }
        public FoxFloat WallAlphaRate { get; set; }
        public FoxPath NormalWallTexPath { get; set; }
        public FoxPath NormalFloorTexPath { get; set; }
        public FoxPath ReflectionCubeMapTexPath { get; set; }
        public FoxPath MaskTexPath { get; set; }
    }
}
