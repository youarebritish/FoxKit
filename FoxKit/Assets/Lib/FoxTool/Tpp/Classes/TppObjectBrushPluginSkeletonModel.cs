using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppObjectBrushPluginSkeletonModel : Data
    {
        // Static properties
        public FoxString ParentDataName { get; set; }
        public FoxBool Visibility { get; set; }
        public List<FoxFilePtr> ModelFile { get; set; }
        public List<FoxFilePtr> GeomFile { get; set; }
        public FoxFilePtr AnimFile { get; set; }
        public FoxFilePtr AnimWindyFile { get; set; }
        public FoxFloat MinSize { get; set; }
        public FoxFloat MaxSize { get; set; }
        public FoxBool IsGeomActivity { get; set; }
        public FoxFloat ThinkOutRate { get; set; }
        public List<FoxFloat> LodLength { get; set; }
    }
}
