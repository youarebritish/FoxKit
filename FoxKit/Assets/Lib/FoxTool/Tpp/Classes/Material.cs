using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class Material : Data
    {
        // Static properties
        public FoxString MaterialName { get; set; }
        public FoxPath Shader { get; set; }
        public FoxPath DiffuseTexture { get; set; }
        public FoxPath SrmTexture { get; set; }
        public FoxPath NormalTexture { get; set; }
        public FoxPath MaterialMapTexture { get; set; }
        public FoxUInt8 MaterialIndex { get; set; }
        public FoxColor DiffuseColor { get; set; }
        public FoxColor SpecularColor { get; set; }
        public FoxPath FmtrPath { get; set; }
        public FoxBool ResidentFlag { get; set; }
    }
}
