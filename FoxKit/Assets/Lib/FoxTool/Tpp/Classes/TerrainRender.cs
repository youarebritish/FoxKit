using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TerrainRender : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxPath FilePath { get; set; }
        public FoxPath LoadFilePath { get; set; }
        public FoxPath DummyFilePath { get; set; }
        public FoxFilePtr FilePtr { get; set; }
        public FoxFloat MeterPerOneRepeat { get; set; }
        public FoxFloat MeterPerPixel { get; set; }
        public FoxBool IsWireFrame { get; set; }
        public FoxBool LodFlag { get; set; }
        public FoxBool IsDebugMaterial { get; set; }
        public List<FoxEntityLink> Materials { get; set; }
        public FoxFloat LodParam { get; set; }
        public List<FoxEntityLink> MaterialConfigs { get; set; }
        public FoxPath PackedAlbedoTexturePath { get; set; }
        public FoxPath PackedNormalTexturePath { get; set; }
        public FoxPath PackedSrmTexturePath { get; set; }
        public FoxUInt64 PackedMaterialIdentify { get; set; }
        public FoxBool IsFourceUsePackedMaterialTexture { get; set; }
        public FoxPath BaseColorTexture { get; set; }
        public FoxFloat MaterialLodScale { get; set; }
        public FoxFloat MaterialLodNearOffset { get; set; }
        public FoxFloat MaterialLodFarOffset { get; set; }
        public FoxFloat MaterialLodHeightOffset { get; set; }
        public FoxBool IsUseWorldTexture { get; set; }
    }
}
