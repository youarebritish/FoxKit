using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class StageLightFadeData : Data
    {
        // Static properties
        public List<FoxEntityLink> LightGroup { get; set; }
        public FoxColor ColorList { get; set; }
        public FoxFloat RequirdTime { get; set; }
    }
}
