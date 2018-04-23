using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GeoTrap : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public List<FoxEntityHandle> Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxEntityLink ConditionArray { get; set; }
        public FoxBool Enable { get; set; }
    }
}
