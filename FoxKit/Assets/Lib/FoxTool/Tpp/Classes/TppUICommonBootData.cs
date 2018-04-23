using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppUICommonBootData : Data
    {
        // Static properties
        public FoxFilePtr UiFile { get; set; }
        public FoxString WindowName { get; set; }
        public FoxInt32 Dependence { get; set; }
        public FoxInt32 PauseFlag { get; set; }
    }
}
