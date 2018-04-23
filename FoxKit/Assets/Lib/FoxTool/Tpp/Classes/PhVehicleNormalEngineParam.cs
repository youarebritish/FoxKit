using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhVehicleNormalEngineParam
    {
        // Static properties
        public List<FoxFloat> SpecPointAngularVelocity { get; set; }
        public List<FoxFloat> SpecPointTorque { get; set; }
        public List<FoxFloat> SpecPointBreakTorque { get; set; }
    }
}
