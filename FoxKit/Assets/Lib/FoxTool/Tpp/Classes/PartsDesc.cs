using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PartsDesc
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString InstanceName { get; set; }
        public FoxFilePtr PartsFile { get; set; }
        public FoxPath ModelPath { get; set; }
        public FoxString PartName { get; set; }
    }
}
