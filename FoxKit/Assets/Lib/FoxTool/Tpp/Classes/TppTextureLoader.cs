using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppTextureLoader : Data
    {
        // Static properties
        public Dictionary<string, FoxPath> Textures { get; set; }
        public FoxPath ForceLargeTextures { get; set; }
    }
}
