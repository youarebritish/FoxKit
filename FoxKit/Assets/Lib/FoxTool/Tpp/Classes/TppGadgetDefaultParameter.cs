using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppGadgetDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat MoveSpeed { get; set; }
        public FoxFloat RotMoveSpeed { get; set; }
        public FoxBool Reverse { get; set; }
        public FoxFloat CurtainSwingDistanceMax { get; set; }
        public FoxFloat CurtainPowerForOffense { get; set; }
        public FoxFloat CurtainPowerForCharacter { get; set; }
        public FoxFloat CurtainDistanceCharacterNear { get; set; }
        public FoxFloat CurtainDistanceCharacterFar { get; set; }
        public FoxFloat CurtainCharacterSearchRange { get; set; }
        public FoxFloat CurtainPushCapsuleRadius { get; set; }
        public FoxFloat CurtainPushCapsuleLengthFront { get; set; }
        public FoxFloat CurtainPushCapsuleLengthRear { get; set; }
        public FoxFloat PhysicsBulletForce { get; set; }
        public FoxFloat PhysicsExplosionForce { get; set; }
        public FoxFloat PhysicsBulletYCorrect { get; set; }
        public FoxFloat FlareDistanceBackward { get; set; }
        public FoxFloat FlareHeight { get; set; }
        public FoxFloat FlareBrightnessLumen { get; set; }
        public FoxFloat FlareLightRange { get; set; }
        public FoxFloat FlareLife { get; set; }
        public FoxFloat FlareStartTime { get; set; }
    }
}
