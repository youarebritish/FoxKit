using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ClipData
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString Name { get; set; }
        public FoxString CameraName { get; set; }
        public FoxInt32 StartFrame { get; set; }
        public FoxInt32 EndFrame { get; set; }
        public FoxInt32 OffsetFrame { get; set; }
    }
}
