using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DecalArray : Data
    {
        // Static properties
        public FoxEntityLink Material { get; set; }
        public FoxInt32 ProjectionMode { get; set; }
        public FoxFloat NearClipScale { get; set; }
        public FoxInt32 ProjectionTarget { get; set; }
        public FoxFloat RepeatU { get; set; }
        public FoxFloat RepeatV { get; set; }
        public FoxFloat Transparency { get; set; }
        public FoxInt32 PolygonDataSource { get; set; }
        public FoxInt32 DrawRejectionLevel { get; set; }
        public FoxFloat DrawRejectionDegree { get; set; }
        public FoxUInt32 DecalFlags { get; set; }
        public List<FoxVector3> Scales { get; set; }
        public List<FoxQuat> Rotations { get; set; }
        public List<FoxVector3> Translations { get; set; }
        public List<FoxEntityLink> Targets { get; set; }
        public List<FoxUInt32> TargetIndices { get; set; }
        public List<FoxUInt32> TargetStartIndices { get; set; }
        public List<FoxInt32> RenderingPriorities { get; set; }
    }
}
