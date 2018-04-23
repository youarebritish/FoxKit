using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhxVehicleAxis : Data
    {
        // Static properties
        public FoxEntityPtr VehicleAxisParam { get; set; }
        public FoxEntityPtr WheelConstraintParam { get; set; }
        public List<FoxEntityPtr> WheelAssociationUnitParams { get; set; }
        public FoxFloat TorqueDistributions { get; set; }
        public FoxFloat GearRatios { get; set; }
    }
}
