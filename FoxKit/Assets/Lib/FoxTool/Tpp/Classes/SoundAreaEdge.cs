using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SoundAreaEdge : Data
    {
        // Static properties
        public FoxEntityPtr Parameter { get; set; }
        public FoxEntityLink PrevArea { get; set; }
        public FoxEntityLink NextArea { get; set; }
    }
}
