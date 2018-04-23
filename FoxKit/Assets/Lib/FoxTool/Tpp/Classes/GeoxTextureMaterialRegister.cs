using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GeoxTextureMaterialRegister : Data
    {
        // Static properties
        public FoxEntityLink MaterialLink { get; set; }
        public FoxString CollisionMaterialName { get; set; }
        public FoxString CollisionColorName { get; set; }
    }
}
