using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GraphxSpatialGraphDataEdge
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityHandle PrevNode { get; set; }
        public FoxEntityHandle NextNode { get; set; }
    }
}
