using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLocationData : Data
    {
        // Static properties
        public FoxUInt16 LocationId { get; set; }
        public FoxPath ScriptPath { get; set; }
        public FoxFilePtr WeatherParametersFile { get; set; }
    }
}
