using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ShearTransformEntity : DataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        // name=shearTransform_shear
        public FoxVector3 ShearTransformShear { get; set; }
    }
}
