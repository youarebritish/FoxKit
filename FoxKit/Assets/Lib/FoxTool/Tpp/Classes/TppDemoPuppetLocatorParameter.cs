using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppDemoPuppetLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public List<FoxString> MotionPoints { get; set; }
        public List<FoxString> MotionEvents { get; set; }
        public Dictionary<string, FoxString> FacialTable { get; set; }
    }
}
