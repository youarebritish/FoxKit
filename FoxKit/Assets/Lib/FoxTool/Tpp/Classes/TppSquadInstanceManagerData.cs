using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSquadInstanceManagerData : Data
    {
        // Static properties
        public FoxPath SquadScriptPath { get; set; }
        public FoxUInt32 SquadSize { get; set; }
        public FoxPath SubSquadScriptPath { get; set; }
        public FoxUInt32 SubSquadSize { get; set; }
    }
}
