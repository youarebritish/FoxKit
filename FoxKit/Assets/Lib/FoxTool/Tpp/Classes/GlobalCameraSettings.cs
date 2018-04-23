using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GlobalCameraSettings : Data
    {
        // Static properties
        public FoxFloat FocalDistance { get; set; }
        public FoxFloat FocalLength { get; set; }
        public FoxFloat Aperture { get; set; }
        public FoxFloat ShutterSpeed { get; set; }
        public FoxUInt32 Flags { get; set; }
    }
}
