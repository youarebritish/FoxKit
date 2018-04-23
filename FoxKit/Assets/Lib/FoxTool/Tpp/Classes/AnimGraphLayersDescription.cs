using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class AnimGraphLayersDescription : Data
    {
        // Static array
        // Dynamic array
        public List<FoxEntityPtr> Layers { get; set; }
    }
}
