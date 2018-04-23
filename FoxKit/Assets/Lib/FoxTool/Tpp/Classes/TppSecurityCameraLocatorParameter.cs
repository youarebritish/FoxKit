using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppSecurityCameraLocatorParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxEntityPtr FileResources { get; set; }
        public FoxEntityLink CharacterFileResources { get; set; }
        public FoxEntityLink CommandPost { get; set; }
        public FoxPath PartsPath { get; set; }
        public FoxUInt32 LifeMax { get; set; }
        public FoxInt32 LifeInitial { get; set; }
        public FoxFloat EyeRange { get; set; }
        public FoxFloat EyeAngleLeftRight { get; set; }
        public FoxFloat EyeAngleUpDown { get; set; }
        public FoxFloat EyeHeadBackDistance { get; set; }
        public FoxFloat ReactionRate { get; set; }
        public FoxFloat MaxCamouflageValue { get; set; }
        public FoxFloat MinCamouflageValue { get; set; }
        public FoxFloat MinYAxisAngle { get; set; }
        public FoxFloat MaxYAxisAngle { get; set; }
        public FoxFloat MinXAxisAngle { get; set; }
        public FoxFloat MaxXAxisAngle { get; set; }
        public FoxFloat XAxisAngle { get; set; }
        public FoxFloat YAxisAngle { get; set; }
        public FoxFloat MoveTime { get; set; }
        public FoxFloat WaitTime { get; set; }
    }
}
