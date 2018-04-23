using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTrapExecSoldierNoticeLightOffCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxString LightSwitchCharacterId { get; set; }
        public List<FoxEntityLink> ReactionAreaBoxShapes { get; set; }
    }
}
