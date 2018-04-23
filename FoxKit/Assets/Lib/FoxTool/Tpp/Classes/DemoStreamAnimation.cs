using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DemoStreamAnimation : Data
    {
        // Static properties
        public FoxPath StreamPath { get; set; }
        public FoxUInt32 DemoLength { get; set; }
        public FoxInt32 LocatorTypes { get; set; }
        public FoxInt32 CameraTypes { get; set; }
        public FoxFilePtr ModelFiles { get; set; }
        public FoxFilePtr HelpBoneFiles { get; set; }
        public FoxFilePtr PartsFiles { get; set; }
        public FoxString ModelPartsDictionary { get; set; }
        public FoxUInt32 UpdateJobCount { get; set; }
        public FoxPath ModelProxyPaths { get; set; }
        public FoxPath PartsProxyPaths { get; set; }
    }
}
