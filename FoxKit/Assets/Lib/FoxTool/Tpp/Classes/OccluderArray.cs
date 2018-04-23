using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class OccluderArray : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFilePtr OccluderArrayFile { get; set; }
    }
}
