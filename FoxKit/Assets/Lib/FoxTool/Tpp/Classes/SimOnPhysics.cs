using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SimOnPhysics : Data
    {
        // Static properties
        public FoxEntityPtr ObjectParam { get; set; }
        public FoxEntityPtr EngineParam { get; set; }
        public FoxEntityPtr Controls { get; set; }
        public Dictionary<string, FoxEntityPtr> SimRootBones { get; set; }
        public Dictionary<string, FoxEntityPtr> SimBones { get; set; }
        public Dictionary<string, FoxEntityPtr> SimTransBones { get; set; }
        public Dictionary<string, FoxEntityPtr> SimHitBones { get; set; }
        public FoxUInt32 FormatVersion { get; set; }
        public FoxEntityLink PhysicsData { get; set; }
    }
}
