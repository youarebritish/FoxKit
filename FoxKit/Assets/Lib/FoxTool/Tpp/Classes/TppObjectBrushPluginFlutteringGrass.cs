using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppObjectBrushPluginFlutteringGrass : Data
    {
        // Static properties
        public FoxString ParentDataName { get; set; }
        public FoxBool Visibility { get; set; }
        public FoxFilePtr ModelFile { get; set; }
        public FoxFloat MinSize { get; set; }
        public FoxFloat MaxSize { get; set; }
        public FoxFloat FarLodSize { get; set; }
        public FoxFloat MiddleLodSize { get; set; }
        public FoxFloat NearLodSize { get; set; }
        public FoxBool EnableLod { get; set; }
        public FoxFloat BaseCycleSpeedRate { get; set; }
        public FoxFloat WindAmplitude { get; set; }
        public FoxFloat WindOffsetFactor { get; set; }
        public FoxBool WindDirYAxisFixZero { get; set; }
    }
}
