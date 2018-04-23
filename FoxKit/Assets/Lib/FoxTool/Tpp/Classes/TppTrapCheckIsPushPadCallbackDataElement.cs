using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapCheckIsPushPadCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxUInt32 PadNumber { get; set; }
        public FoxString CheckButtons { get; set; }
    }
}
