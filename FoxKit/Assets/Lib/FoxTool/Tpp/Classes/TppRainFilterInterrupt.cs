using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppRainFilterInterrupt : Data
    {
        // Static properties
        public List<FoxMatrix4> PlaneMatrices { get; set; }
        public List<FoxPath> MaskTextures { get; set; }
    }
}
