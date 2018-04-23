using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppMarkCharacterInterrogationParamData : Data
    {
        // Static properties
        public FoxEntityLink TargetCharacters { get; set; }
        public FoxString TargetCharacterIds { get; set; }
        public FoxUInt8 Radius { get; set; }
        public FoxInt32 GoalType { get; set; }
    }
}
