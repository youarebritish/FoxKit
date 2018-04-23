using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppPhaseControlParameter : Data
    {
        // Static properties
        public FoxFloat Range { get; set; }
        public FoxFloat Sneak1Range { get; set; }
        public FoxFloat Sneak2Range { get; set; }
        public FoxFloat Sneak3Range { get; set; }
        public FoxFloat MarginLastNearChara { get; set; }
    }
}
