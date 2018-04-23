using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppAmbientParameter : Data
    {
        // Static properties
        public FoxString AmbientEvent { get; set; }
        public FoxString ObjectRtpcName { get; set; }
        public FoxFloat ObjectRtpcValue { get; set; }
        public FoxFloat AuxSends { get; set; }
        public FoxFloat DryVolume { get; set; }
    }
}
