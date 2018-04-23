using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class FxLocatorData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxString EffectInstanceName { get; set; }
        public FoxBool EnableUserRandomSeed { get; set; }
        public FoxUInt32 UserRandomSeed { get; set; }
        public FoxBool ShapeKeep { get; set; }
        public FoxBool CreateOnInitialize { get; set; }
        public FoxBool BlockMemoryAllocation { get; set; }
        public FoxFilePtr VfxFile { get; set; }
    }
}
