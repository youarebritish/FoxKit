using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppGroupVehicleRouteInfo : Data
    {
        // Static properties
        public FoxString VehicleRouteId { get; set; }
        public FoxBool IsPreferredMove { get; set; }
        public FoxInt32 VehicleType { get; set; }
        public List<FoxString> MemberRouteIds { get; set; }
    }
}
