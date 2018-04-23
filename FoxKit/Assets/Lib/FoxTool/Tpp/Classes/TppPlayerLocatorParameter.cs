using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppPlayerLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxUInt32 LifeMax { get; set; }
        public FoxUInt32 StaminaMax { get; set; }
        public FoxPath DamageSet { get; set; }
        public FoxString InitBelongings { get; set; }
        public FoxString InitActiveWeapon { get; set; }
        public FoxString CameraPresetName { get; set; }
        public FoxEntityLink InitCameraTarget { get; set; }
        public FoxFloat MagazineSelectableCost { get; set; }
        public FoxBool EnableCoreInfo { get; set; }
        // name=TODO_padNo
        public FoxUInt32 TodoPadNo { get; set; }
        public FoxPath PartsPath { get; set; }
        public FoxPath FpkPath { get; set; }
        public FoxString Type { get; set; }
    }
}
