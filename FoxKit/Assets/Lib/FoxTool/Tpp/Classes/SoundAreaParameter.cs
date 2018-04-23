using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SoundAreaParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString AmbientEvent { get; set; }
        public FoxString AmbientRtpcName { get; set; }
        public FoxFloat AmbientRtpcValue { get; set; }
        public FoxString ObjectRtpcName { get; set; }
        public FoxFloat ObjectRtpcValue { get; set; }
        public FoxFloat AuxSends { get; set; }
        public FoxFloat DryVolume { get; set; }
    }
}
