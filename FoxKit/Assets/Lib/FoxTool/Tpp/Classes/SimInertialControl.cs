using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SimInertialControl
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString Bones { get; set; }
        public FoxEntityPtr ControlParam { get; set; }
    }
}
