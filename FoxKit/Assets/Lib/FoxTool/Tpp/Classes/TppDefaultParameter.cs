using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppDefaultParameter : Data
    {
        // Static properties
        public FoxString Id { get; set; }
        public FoxEntityPtr Params { get; set; }
        public FoxString GroupName { get; set; }
    }
}
