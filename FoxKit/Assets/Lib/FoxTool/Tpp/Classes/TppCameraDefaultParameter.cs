using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppCameraDefaultParameter
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString ParamName { get; set; }
        public FoxFloat AroundCameraZoomRate { get; set; }
        public FoxFloat AroundCameraZoomIntepRata { get; set; }
        public FoxFloat RotYCorrectLimitDegreeYMax { get; set; }
        public FoxFloat RotYCorrectRate { get; set; }
        public FoxFloat RotXCorrectLimitDegreeYMax { get; set; }
        public FoxFloat RotXCorrectRate { get; set; }
        public FoxFloat DefaultRotXDegree { get; set; }
        public FoxBool IsValidShake { get; set; }
        public FoxVector3 DefaultShakePosRange { get; set; }
        public FoxVector3 DefaultShakeRotRange { get; set; }
        public FoxFloat DefaultShakePosCycle { get; set; }
        public FoxFloat DefaultShakeRotCycle { get; set; }
        public FoxInt32 DefaultShakeInterporate { get; set; }
        public FoxFloat StrykerAroundDistance { get; set; }
        public FoxFloat StrykerAroundFocalLength { get; set; }
        public FoxVector3 StrykerAroundOffset { get; set; }
        public FoxFloat StandSubjectFocalLength { get; set; }
        public FoxFloat StandSubjectFocalInterpRate { get; set; }
        public FoxFloat SquatSubjectFocalLength { get; set; }
        public FoxFloat SquatSubjectFocalInterpRate { get; set; }
        public FoxFloat CrawlSubjectFocalLength { get; set; }
        public FoxFloat CrawlSubjectFocalInterpRate { get; set; }
        public FoxFloat CrawlMoveSubjectFocalLength { get; set; }
        public FoxFloat CrawlMoveSubjectFocalInterpRate { get; set; }
        public FoxFloat SquatMoveSubjectFocalLength { get; set; }
        public FoxFloat SquatMoveSubjectFocalInterpRate { get; set; }
        public FoxFloat StandWalkSubjectFocalLength { get; set; }
        public FoxFloat StandWalkSubjectFocalInterpRate { get; set; }
        public FoxFloat StandRunSubjectFocalLength { get; set; }
        public FoxFloat StandRunSubjectFocalInterpRate { get; set; }
        public FoxFloat DefaultCameraOffsetDistance { get; set; }
        public FoxFloat StandCameraOffsetDistance { get; set; }
        public FoxFloat SquatCameraOffsetDistance { get; set; }
        public FoxFloat CrawlCameraOffsetDistance { get; set; }
        public FoxFloat CrawlMoveCameraOffsetDistance { get; set; }
        public FoxFloat SquatMoveCameraOffsetDistance { get; set; }
        public FoxFloat StandWalkCameraOffsetDistance { get; set; }
        public FoxFloat StandRunCameraOffsetDistance { get; set; }
        public FoxFloat TestTpsDistance { get; set; }
        public FoxFloat TestTpsFocalLength { get; set; }
        public FoxVector3 TestTpsOffset { get; set; }
        public FoxFloat NearClip { get; set; }
        public FoxFloat FarClip { get; set; }
    }
}
