using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppHelicopterRendezvousPoint : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxString DropRouteId { get; set; }
        public FoxString ReturnRouteId { get; set; }
        public FoxQuat AdditionalRotation { get; set; }
        public FoxUInt8 Tags { get; set; }
    }
}
