using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ChFileResourceContainer : DataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public Dictionary<string, FoxFilePtr> Resources { get; set; }
    }
}
