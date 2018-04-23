using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppDoorLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxString DamageSetType { get; set; }
        public FoxPath PartsName { get; set; }
        public FoxInt32 Type { get; set; }
        public FoxBool HaveLock { get; set; }
        public FoxBool IsLocked { get; set; }
        public FoxBool EnablePicking { get; set; }
        public FoxBool IsOneWayPicking { get; set; }
        public FoxBool UseMotionOpenClose { get; set; }
        public FoxBool IsSlideDoor { get; set; }
        public FoxBool IsVerticalSlide { get; set; }
        public FoxBool DisalbeRealize { get; set; }
        public FoxFloat TimeToOpen { get; set; }
        public FoxFloat WaitTimeToClose { get; set; }
        public FoxString SoundType { get; set; }
        public FoxString OwnerType { get; set; }
        public FoxString TacticalActionEdgeId { get; set; }
    }
}
