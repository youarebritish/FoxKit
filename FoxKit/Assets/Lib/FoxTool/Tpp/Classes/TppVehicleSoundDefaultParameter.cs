using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppVehicleSoundDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxString Engine { get; set; }
        public FoxString EngineKill { get; set; }
        public FoxString BackGear { get; set; }
        public FoxString SuspensionLeft { get; set; }
        public FoxString SuspensionRight { get; set; }
        public FoxString RoadNoiseNormalLeft { get; set; }
        public FoxString RoadNoiseNormalRight { get; set; }
        public FoxString RoadNoiseSkidLeft { get; set; }
        public FoxString RoadNoiseSkidRight { get; set; }
    }
}
