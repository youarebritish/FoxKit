using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class EventDataUnit : Data
    {
        // Static properties
        public FoxString EventName { get; set; }
        public FoxEntityPtr Sections { get; set; }
        public List<FoxString> ParamString { get; set; }
        public List<FoxInt32> ParamInt { get; set; }
        public FoxFloat ParamFloat { get; set; }
    }
}
