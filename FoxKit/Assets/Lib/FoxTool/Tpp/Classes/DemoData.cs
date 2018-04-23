using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DemoData : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFilePtr EvfFiles { get; set; }
        public FoxBool OnMemory { get; set; }
        public FoxInt32 DemoLength { get; set; }
        public FoxInt32 Priority { get; set; }
        public FoxPath ScriptPath { get; set; }
        public FoxFilePtr FmdlFiles { get; set; }
        public FoxFilePtr HelpBoneFiles { get; set; }
        public Dictionary<string, FoxEntityPtr> PartsDesc { get; set; }
        public FoxEntityPtr ClipDatas { get; set; }
        public FoxPath LoadFiles { get; set; }
        public FoxString DemoId { get; set; }
        public FoxEntityLink PlayingRoot { get; set; }
        public FoxEntityLink StreamAnimation { get; set; }
        public FoxPath DemoStreamPath { get; set; }
        public FoxPath MotionPath { get; set; }
        public FoxFilePtr MotionFile { get; set; }
        public FoxPath AudioPath { get; set; }
        public FoxFilePtr SubtitleFile { get; set; }
        public FoxFilePtr SubtitleBinaryFile { get; set; }
        public FoxString StringParams { get; set; }
        public Dictionary<string, FoxEntityLink> EntityParams { get; set; }
        public FoxFilePtr FileParams { get; set; }
        public Dictionary<string, FoxEntityPtr> ControlCharacters { get; set; }
        public Dictionary<string, FoxEntityLink> ControlDatas { get; set; }
        public FoxString ControlCollectibles { get; set; }
        public Dictionary<string, FoxEntityPtr> Parameters { get; set; }
        public FoxString SetupLights { get; set; }
        public FoxInt32 CameraInterpType { get; set; }
        public FoxInt32 CameraInterpFrame { get; set; }
        public FoxFloat CameraInterpCurveRate { get; set; }
        public FoxFloat CameraInterpScurveCenter { get; set; }
        public FoxVector3 CameraTranslation { get; set; }
        public FoxQuat CameraRotation { get; set; }
        public FoxFloat CameraParam { get; set; }
        public FoxVector3 CameraStartTranslation { get; set; }
        public FoxQuat CameraStartRotation { get; set; }
        public FoxFloat CameraStartParam { get; set; }
        public FoxInt32 EventCacheNum { get; set; }
        public FoxInt32 EventInterpCacheNum { get; set; }
        public FoxInt32 EventSkipCacheNum { get; set; }
        public FoxString HighestTextureStreamModel { get; set; }
        public FoxPath HighestTexture { get; set; }
        public FoxInt32 ObjectNum { get; set; }
        public FoxEntityLink BlockPositionSetter { get; set; }
    }
}
