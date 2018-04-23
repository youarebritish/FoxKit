using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ChCharacterLocatorData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxBool Enable { get; set; }
        public FoxPath ScriptPath { get; set; }
        public FoxEntityPtr Params { get; set; }
        public FoxEntityPtr ObjectCreator { get; set; }
        public FoxUInt8 Tags { get; set; }
        public FoxString CharacterId { get; set; }
    }
}
