using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapCheckIsInViewCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxEntityLink CheckLocator { get; set; }
        public FoxFloat CheckAreaSize { get; set; }
    }
}
