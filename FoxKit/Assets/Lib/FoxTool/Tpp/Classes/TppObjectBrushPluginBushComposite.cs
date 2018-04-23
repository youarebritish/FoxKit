using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppObjectBrushPluginBushComposite : Data
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
        public FoxInt32 InteractionType { get; set; }
        public FoxFloat ScaleDefaultY { get; set; }
        public FoxFloat ScaleDefaultXZ { get; set; }
        public FoxFloat ScaleRange { get; set; }
        public FoxFloat ModelRotY { get; set; }
        public FoxFloat ModelRotRange { get; set; }
        public FoxFloat BlockWidth { get; set; }
        public FoxFloat BlockHeight { get; set; }
        public FoxUInt32 RandomSeed { get; set; }
        public FoxEntityPtr GimmickParameter { get; set; }
        public FoxEntityLink ExternalGimmickParameter { get; set; }
        public List<FoxEntityPtr> CompositeDataElementArray { get; set; }
    }
}
