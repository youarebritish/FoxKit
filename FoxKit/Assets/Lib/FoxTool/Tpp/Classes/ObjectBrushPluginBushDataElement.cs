using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ObjectBrushPluginBushDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString LodMeshName { get; set; }
        public FoxFloat LodDistance { get; set; }
        public FoxEntityPtr LodParameter { get; set; }
        public FoxVector3 CollidePosOffset { get; set; }
        public FoxFloat BlockWidth { get; set; }
        public FoxFloat BlockHeight { get; set; }
        public FoxFloat BaseDensity { get; set; }
        public FoxFloat CamoufDensity { get; set; }
        public FoxFloat LayDownRange { get; set; }
        public FoxFloat LayDownAngleMax { get; set; }
        public FoxFloat LayDownAngle { get; set; }
        public FoxFloat MinLayDownTime { get; set; }
        public FoxFloat MaxLayDownTime { get; set; }
        public FoxFloat LayDownTimeRate { get; set; }
        public FoxFloat LayDownRecoverVel { get; set; }
        public FoxFloat LayDownVelMax { get; set; }
        public FoxFloat GetUpVelMax { get; set; }
        public FoxFloat RotateVelMax { get; set; }
        public FoxFloat ElasticForce { get; set; }
        public FoxFloat AlphaMinimizeDist { get; set; }
        public FoxFloat AlphaMaximizeDist { get; set; }
        public FoxFloat ModelRadius { get; set; }
        public FoxString NoiseSeType { get; set; }
        public FoxUInt32 BushFlags { get; set; }
        public FoxFloat CollideActiveDistance { get; set; }
        public FoxFloat WindActiveDistance { get; set; }
        public FoxFloat NoiseActiveDistance { get; set; }
        public FoxFloat MatrixActiveDistance { get; set; }
        public FoxFloat AlphaActiveDistance { get; set; }
        public FoxFloat BaseCycleSpeedRate { get; set; }
        public FoxFloat WindAmplitude { get; set; }
        public FoxFloat RandomAgainstWind { get; set; }
        public FoxFloat UnderpartDensity { get; set; }
        public FoxFloat UnderpartHeight { get; set; }
        public FoxFloat PushDistanceRate { get; set; }
        public FoxFloat DirectionLimit { get; set; }
        public FoxBool WindDirYAxisFixZero { get; set; }
        public FoxFloat WindOffsetFactor { get; set; }
        public FoxFilePtr BulletEffect { get; set; }
        public FoxFilePtr FairEffect { get; set; }
        public FoxFilePtr RainEffect { get; set; }
    }
}
