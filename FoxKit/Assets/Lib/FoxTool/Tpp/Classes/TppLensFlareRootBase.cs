using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLensFlareRootBase : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxUInt32 MemoryAllocateSize { get; set; }
        public FoxBool UseDebugLightPosition { get; set; }
        public FoxFloat LightPositionX { get; set; }
        public FoxFloat LightPositionY { get; set; }
        public FoxFloat ShieldCheckLength { get; set; }
        public List<FoxEntityLink> Shapes { get; set; }
    }
}
