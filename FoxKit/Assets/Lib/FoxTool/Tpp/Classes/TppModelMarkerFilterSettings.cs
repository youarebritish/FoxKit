using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppModelMarkerFilterSettings : Data
    {
        // Static properties
        public FoxFloat TexRepeatsNear { get; set; }
        public FoxFloat TexRepeatsFar { get; set; }
        public FoxFloat TexRepeatsMin { get; set; }
        public FoxFloat TexRepeatsMax { get; set; }
        public List<FoxVector3> Alphas { get; set; }
        public List<FoxVector3> Offsets { get; set; }
        public List<FoxVector3> Incidences { get; set; }
    }
}
