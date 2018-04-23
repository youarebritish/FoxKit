using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TerrainDecalPack : Data
    {
        // Static properties
        public FoxFilePtr TerrainDecalPackFile { get; set; }
        public List<FoxEntityLink> MaterialLinks { get; set; }
    }
}
