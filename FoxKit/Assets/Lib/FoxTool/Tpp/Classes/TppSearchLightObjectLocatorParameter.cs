using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSearchLightObjectLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public FoxInt32 Emptype { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxInt32 Life { get; set; }
        public FoxFloat MaxXAxisAngle { get; set; }
        public FoxFloat MinXAxisAngle { get; set; }
        public FoxFloat MaxYAxisAngle { get; set; }
        public FoxFloat MinYAxisAngle { get; set; }
        public FoxEntityLink InitTarget { get; set; }
        public FoxFloat RangeDiscovery { get; set; }
        public FoxFloat AngleLeftRightDiscovery { get; set; }
        public FoxFloat AngleUpDownDiscovery { get; set; }
        public FoxFloat HeadBackDistanceDiscovery { get; set; }
        public FoxFloat RangeDim { get; set; }
        public FoxFloat AngleLeftRightDim { get; set; }
        public FoxFloat AngleUpDownDim { get; set; }
        public FoxFloat HeadBackDistanceDim { get; set; }
        public FoxBool IsSwichOnStart { get; set; }
    }
}
