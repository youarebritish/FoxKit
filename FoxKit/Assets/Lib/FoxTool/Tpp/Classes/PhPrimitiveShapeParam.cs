using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhPrimitiveShapeParam
    {
        // Static properties
        public FoxVector3 Offset { get; set; }
        public FoxQuat Rotation { get; set; }
        public FoxVector3 Size { get; set; }
        public FoxInt32 Type { get; set; }
    }
}
