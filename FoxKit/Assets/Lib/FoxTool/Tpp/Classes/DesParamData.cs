using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DesParamData : Data
    {
        // Static properties
        public FoxFloat Density { get; set; }
        public FoxFloat Friction { get; set; }
        public FoxFloat Restitution { get; set; }
        public FoxString MaterialName { get; set; }
        public FoxInt32 DesCondition { get; set; }
        public FoxFloat DesImpactPowerThreshold { get; set; }
        public FoxFloat PhysicalCoefficient { get; set; }
    }
}
