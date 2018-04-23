using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DataIdentifier : Data
    {
        // Static properties
        public FoxString Identifier { get; set; }
        public Dictionary<string, FoxEntityLink> Links { get; set; }
    }
}
