using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSpecialCharacterInterrogationData : Data
    {
        // Static properties
        public FoxPath DataTablePath { get; set; }
        public Dictionary<string, FoxEntityLink> InterroParams { get; set; }
    }
}
