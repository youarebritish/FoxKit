using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapExecThunderLightningCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxEntityLink PositionLocator { get; set; }
        public FoxFloat Power { get; set; }
    }
}
