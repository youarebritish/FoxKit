using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppCombatLocatorData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFloat Radius { get; set; }
        public FoxFloat LostSearchRadius { get; set; }
        public FoxBool IsUseWaitPrioirty { get; set; }
        public FoxEntityLink SubLocators { get; set; }
    }
}
