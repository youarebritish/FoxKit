using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GeoxTargetDesc : Data
    {
        // Static properties
        public FoxVector3 PosArray { get; set; }
        public FoxVector3 ScaleArray { get; set; }
        public FoxQuat RotArray { get; set; }
        public FoxUInt32 PrimTypeArray { get; set; }
        public FoxUInt64 SystemAttributeArray { get; set; }
        public FoxUInt32 ThroughValueArray { get; set; }
        public FoxUInt32 FlagArray { get; set; }
        public FoxString CategoryTag { get; set; }
        public FoxString NameArray { get; set; }
        public FoxString AttachSkeletonArray { get; set; }
        public List<FoxString> GroupArray { get; set; }
        public List<FoxUInt32> ObjIndexForgroupTagArray { get; set; }
        public FoxEntityLink ApplicationDataLinkArray { get; set; }
    }
}
