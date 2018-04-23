using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapExecChangeGlobalWindCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxFloat Speed { get; set; }
        public FoxQuat Rotation { get; set; }
        public FoxFloat SpeedTurbulentRate { get; set; }
        public FoxFloat SpeedTurbulentCycle { get; set; }
        public FoxFloat RotTurbulentRate { get; set; }
        public FoxFloat RotTurbulentCycle { get; set; }
        public FoxFloat InterpTime { get; set; }
    }
}
