using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppElectricCableData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFloat Size { get; set; }
        public FoxEntityLink Locator0 { get; set; }
        public FoxEntityLink Locator1 { get; set; }
        public FoxFilePtr PartsFile { get; set; }
        public FoxPath AnimFile { get; set; }
        public FoxFilePtr AnimFilePtr { get; set; }
    }
}
