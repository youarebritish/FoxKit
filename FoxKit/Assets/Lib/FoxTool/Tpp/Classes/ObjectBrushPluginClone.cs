using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ObjectBrushPluginClone : Data
    {
        // Static properties
        public FoxString ParentDataName { get; set; }
        public FoxBool Visibility { get; set; }
        public FoxFilePtr ModelFile { get; set; }
        public FoxFloat MinSize { get; set; }
        public FoxFloat MaxSize { get; set; }
        public FoxFloat FarLodSize { get; set; }
        public FoxFloat MiddleLodSize { get; set; }
        public FoxFloat NearLodSize { get; set; }
        public FoxBool EnableLod { get; set; }
    }
}
