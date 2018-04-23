using System.Collections.Generic;
using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhxAssociation : Data
    {
        // Static properties
        public FoxEntityLink PhysicsData { get; set; }
        public Dictionary<string, FoxEntityPtr> Connections { get; set; }
        public FoxEntityPtr Param { get; set; }
    }
}
