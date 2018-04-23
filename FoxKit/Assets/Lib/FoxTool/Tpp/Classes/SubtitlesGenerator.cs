using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SubtitlesGenerator : Data
    {
        // Static properties
        public FoxFilePtr Files { get; set; }
        public FoxFilePtr RawFiles { get; set; }
        public FoxString Key { get; set; }
        public FoxColor Color { get; set; }
        public FoxVector3 Offset { get; set; }
        public FoxVector3 Size { get; set; }
        public FoxFloat FontSpace { get; set; }
        public FoxFloat LineSpace { get; set; }
        public FoxInt32 HAlign { get; set; }
        public FoxInt32 VAlign { get; set; }
        public FoxInt32 BAlign { get; set; }
        public FoxString FontName { get; set; }
        public FoxBool AutoLineFeed { get; set; }
    }
}
