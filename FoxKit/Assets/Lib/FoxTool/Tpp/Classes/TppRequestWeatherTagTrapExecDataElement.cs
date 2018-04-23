using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppRequestWeatherTagTrapExecDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxString TagName { get; set; }
        public FoxUInt8 Priority { get; set; }
        public FoxFloat InterpTime { get; set; }
    }
}
