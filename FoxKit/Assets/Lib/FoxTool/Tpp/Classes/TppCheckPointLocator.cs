using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppCheckPointLocator : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFloat Size { get; set; }
        public FoxUInt16 CheckPointId { get; set; }
        public FoxBool UsePlayerRotation { get; set; }
    }
}
