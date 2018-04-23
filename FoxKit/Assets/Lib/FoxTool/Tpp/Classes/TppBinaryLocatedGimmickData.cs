using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppBinaryLocatedGimmickData : Data
    {
        // Static properties
        public FoxFilePtr ModelFile { get; set; }
        public FoxFilePtr GeomFile { get; set; }
        public FoxFilePtr BreakedModelFile { get; set; }
        public FoxFilePtr BreakedGeomFile { get; set; }
        public FoxFilePtr PartsFile { get; set; }
        public FoxUInt32 NumDynamicGimmick { get; set; }
        public FoxUInt32 NumBreakedModelLimit { get; set; }
        public FoxFilePtr LocaterFile { get; set; }
        public FoxUInt32 Flags1 { get; set; }
        public FoxUInt32 Flags2 { get; set; }
    }
}
