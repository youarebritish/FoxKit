using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppEspionageRadioLocator : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxString RadioLocatorId { get; set; }
        public FoxBool Enable { get; set; }
        public FoxFloat MinDistance { get; set; }
        public FoxFloat MaxDistance { get; set; }
        public FoxFloat Angle { get; set; }
    }
}
