using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppMissionParameterData : Data
    {
        // Static properties
        public FoxFilePtr MissionSetupFile { get; set; }
        public FoxBool Enable { get; set; }
    }
}
