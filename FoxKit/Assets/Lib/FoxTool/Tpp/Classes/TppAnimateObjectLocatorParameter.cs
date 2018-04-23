using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppAnimateObjectLocatorParameter : TppGadgetLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxBool IsSleepStart { get; set; }
        public FoxBool DisalbeRealize { get; set; }
    }
}
