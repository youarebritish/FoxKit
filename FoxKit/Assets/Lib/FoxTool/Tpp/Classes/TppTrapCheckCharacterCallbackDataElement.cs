using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapCheckCharacterCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxString CharacterIdArray { get; set; }
        public FoxString CharacterTypeStringArray { get; set; }
        public FoxUInt32 CharacterTypeArray { get; set; }
    }
}
