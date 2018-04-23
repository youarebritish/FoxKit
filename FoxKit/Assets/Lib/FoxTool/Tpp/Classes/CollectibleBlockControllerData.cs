using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class CollectibleBlockControllerData : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxBool EnableCustomization { get; set; }
        public FoxUInt32 CommonBlockSize { get; set; }
        public FoxPath CommonBlockPath { get; set; }
        public FoxUInt32 PrimaryWeaponBlockSize { get; set; }
        public FoxUInt32 SecondaryWeaponBlockSize { get; set; }
        public FoxUInt32 MissionWeaponBlockCount { get; set; }
        public FoxUInt32 MissionWeaponBlockSize { get; set; }
        public FoxUInt32 SupportWeaponBlockCount { get; set; }
        public FoxUInt32 SupportWeaponBlockSize { get; set; }
    }
}
