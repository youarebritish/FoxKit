using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppAttackEmplacementLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public FoxInt32 Emptype { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxInt32 Life { get; set; }
        public FoxFloat MaxXAxisAngle { get; set; }
        public FoxFloat MinXAxisAngle { get; set; }
        public FoxFloat MaxYAxisAngle { get; set; }
        public FoxFloat MinYAxisAngle { get; set; }
        public FoxEntityLink InitTarget { get; set; }
        public FoxString WeaponName { get; set; }
    }
}
