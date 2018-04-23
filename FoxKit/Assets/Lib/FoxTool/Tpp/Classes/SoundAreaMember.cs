using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SoundAreaMember : Data
    {
        // Static properties
        public FoxEntityLink Shapes { get; set; }
        public FoxUInt32 Priority { get; set; }
        public FoxEntityPtr Parameter { get; set; }
    }
}
