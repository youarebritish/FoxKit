using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class StaticModelArrayLinkTarget : Data
    {
        // Static properties
        public FoxEntityHandle StaticModelArray { get; set; }
        public FoxUInt32 ArrayIndex { get; set; }
    }
}
