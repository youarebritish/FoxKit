using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class UiFontDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString Language { get; set; }
        public FoxString FontName { get; set; }
        public FoxFilePtr FontFile { get; set; }
        public FoxPath TexturePath { get; set; }
        public FoxFloat FontWidth { get; set; }
        public FoxFloat FontHeight { get; set; }
        public FoxFloat TextSpace { get; set; }
        public FoxFloat LineSpace { get; set; }
        public FoxVector4 FontEdge { get; set; }
    }
}
