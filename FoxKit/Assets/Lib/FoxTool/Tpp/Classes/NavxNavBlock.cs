using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class NavxNavBlock : Data
    {
        // Static properties
        public FoxString SceneName { get; set; }
        public FoxString WorldName { get; set; }
        public FoxUInt32 TileId { get; set; }
        public FoxPath FilePath { get; set; }
        public FoxFilePtr FilePtr { get; set; }
        public FoxFilePtr RemainingFilePtr { get; set; }
        public FoxBool IsSplit { get; set; }
    }
}
