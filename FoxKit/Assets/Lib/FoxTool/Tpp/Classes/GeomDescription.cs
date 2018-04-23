using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GeomDescription : Data
    {
        // Static properties
        public FoxEntityLink Depends { get; set; }
        public FoxString PartName { get; set; }
        public FoxString BuildType { get; set; }
        public FoxFilePtr GeomFile { get; set; }
        public FoxString SkeletonName { get; set; }
        public FoxVector3 OffsetScale { get; set; }
        public FoxQuat OffsetRotQuat { get; set; }
        public FoxVector3 OffsetTranslation { get; set; }
    }
}
