using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ModelDescription : Data
    {
        // Static properties
        public FoxEntityLink Depends { get; set; }
        public FoxString PartName { get; set; }
        public FoxString BuildType { get; set; }
        public FoxFilePtr ModelFile { get; set; }
        public FoxFilePtr ConnectPointFile { get; set; }
        public FoxFilePtr GameRigFile { get; set; }
        public FoxFilePtr HelpBoneFile { get; set; }
        public FoxFilePtr LipAdjustFile { get; set; }
        public FoxFilePtr FacialSettingFile { get; set; }
        public FoxString InvisibleMeshNames { get; set; }
        public FoxFloat LodFarPixelSize { get; set; }
        public FoxFloat LodNearPixelSize { get; set; }
        public FoxFloat LodPolygonSize { get; set; }
        public FoxInt32 DrawRejectionLevel { get; set; }
        public FoxInt32 RejectFarRangeShadowCast { get; set; }
    }
}
