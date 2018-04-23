using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppLadderData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxInt32 LadderType { get; set; }
        public FoxUInt32 NumSteps { get; set; }
        public FoxFloat StepInterval { get; set; }
        public FoxString TacticalActionId { get; set; }
        public FoxString Motion { get; set; }
        public List<FoxEntityLink> EntryPoints { get; set; }
    }
}
