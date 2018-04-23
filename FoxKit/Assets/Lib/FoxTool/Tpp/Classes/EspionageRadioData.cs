using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class EspionageRadioData : Data
    {
        // Static properties
        public FoxString Key { get; set; }
        public FoxEntityLink TargetArray { get; set; }
        public FoxString RadioGroupNameArray { get; set; }
        public FoxBool IsOn { get; set; }
        public FoxBool IsOnce { get; set; }
        public FoxBool EnableSendMessage { get; set; }
    }
}
