using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppHumanEnemyMissionParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat EyeParamUpStartHour { get; set; }
        public FoxFloat EyeParamUpEndHour { get; set; }
        public FoxFloat EyeParamDownStartHour { get; set; }
        public FoxFloat EyeParamDownEndHour { get; set; }
    }
}
