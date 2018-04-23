using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhObjectDesc : Data
    {
        // Static properties
        public List<FoxEntityPtr> Bodies { get; set; }
        public List<FoxEntityPtr> Constraints { get; set; }
        public List<FoxInt32> BodyIndices { get; set; }
    }
}
