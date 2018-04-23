using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhxVehicleNormalEngine : Data
    {
        // Static properties
        public List<FoxEntityLink> VehicleAxes { get; set; }
        public FoxFloat TorqueDistributions { get; set; }
        public FoxFloat GearRatios { get; set; }
        public FoxEntityPtr VehicleNormalEngineParam { get; set; }
    }
}
