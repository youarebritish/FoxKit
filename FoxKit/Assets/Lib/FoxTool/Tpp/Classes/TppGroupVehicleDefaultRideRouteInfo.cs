using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppGroupVehicleDefaultRideRouteInfo : Data
    {
        // Static properties
        public FoxString VehicleRouteId { get; set; }
        public FoxBool IsPreferredMove { get; set; }
        public FoxEntityLink Vehicle { get; set; }
        public FoxEntityLink Members { get; set; }
    }
}
