using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppVfxFileLoader : Data
    {
        // Static properties
        public FoxFilePtr VfxFiles { get; set; }
        public FoxFilePtr GeoMaterialFiles { get; set; }
        public FoxFilePtr OtherFiles { get; set; }
    }
}
