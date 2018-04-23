using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class SoundPackage : Data
    {
        // Static properties
        public FoxFilePtr SoundDataFile { get; set; }
        public FoxBool SyncLoad { get; set; }
    }
}
