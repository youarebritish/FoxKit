using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SimClothSettingData : Data
    {
        // Static properties
        public FoxUInt32 Iteration { get; set; }
        public FoxFloat Mass { get; set; }
        public FoxFloat VelocityDamp { get; set; }
        public FoxFloat AirResistance { get; set; }
        public FoxFloat GravityRate { get; set; }
        public FoxFloat RestoreRate { get; set; }
        public FoxFloat InertialRate { get; set; }
        public FoxFloat InertialMax { get; set; }
        public FoxBool NoHitSkinMesh { get; set; }
    }
}
