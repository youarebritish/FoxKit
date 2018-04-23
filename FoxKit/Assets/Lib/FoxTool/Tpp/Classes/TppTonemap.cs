using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTonemap : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxFloat Threshold { get; set; }
        public FoxFloat Range { get; set; }
    }
}
