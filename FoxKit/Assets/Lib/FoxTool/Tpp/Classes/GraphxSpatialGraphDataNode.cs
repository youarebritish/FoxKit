using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GraphxSpatialGraphDataNode
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxVector3 Position { get; set; }
        public FoxEntityHandle Inlinks { get; set; }
        public List<FoxEntityHandle> Outlinks { get; set; }
    }
}
