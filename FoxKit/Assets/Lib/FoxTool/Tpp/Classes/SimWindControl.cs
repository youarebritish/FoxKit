using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SimWindControl
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public List<FoxString> Bones { get; set; }
        public FoxEntityPtr ControlParam { get; set; }
    }
}
