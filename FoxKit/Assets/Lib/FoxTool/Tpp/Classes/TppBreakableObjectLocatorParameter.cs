using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppBreakableObjectLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxInt32 Life { get; set; }
        public FoxEntityLink ChildBreakableObject { get; set; }
        public FoxString ExplodeWeaponId { get; set; }
        public FoxString InitialNormalMeshNames { get; set; }
        public FoxString LeftBrokenMeshNames { get; set; }
        public FoxBool DisalbeRealize { get; set; }
        public FoxString PathIdArray { get; set; }
    }
}
