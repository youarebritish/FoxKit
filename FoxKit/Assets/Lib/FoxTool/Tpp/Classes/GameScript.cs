using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GameScript : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxPath ScriptPath { get; set; }
        public FoxEntityLink Variables { get; set; }
    }
}
