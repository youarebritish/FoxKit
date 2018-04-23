using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSwitchGadgetLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public List<FoxEntityLink> RelatedObjects { get; set; }
        public FoxBool InitialOn { get; set; }
    }
}
