using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class Group : Data
    {
        // Static properties
        public FoxEntityLink ParentGroup { get; set; }
        public List<FoxEntityLink> Members { get; set; }
        public FoxBool DeleteFromPackage { get; set; }
    }
}
