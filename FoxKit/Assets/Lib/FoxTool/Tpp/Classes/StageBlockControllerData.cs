using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class StageBlockControllerData : Data
    {
        // Static properties
        public FoxBool Enable { get; set; }
        public FoxString StageName { get; set; }
        public FoxBool UseBaseDirectoryPathAndName { get; set; }
        public FoxString BaseDirectoryPath { get; set; }
        public FoxString BaseName { get; set; }
        public FoxUInt32 BlockSizeX { get; set; }
        public FoxUInt32 BlockSizeZ { get; set; }
        public FoxUInt32 CountX { get; set; }
        public FoxUInt32 CountZ { get; set; }
        public FoxUInt32 CenterIndexX { get; set; }
        public FoxUInt32 CenterIndexZ { get; set; }
        public FoxUInt32 BlockSizeInBytes { get; set; }
        public FoxUInt32 BlockMarginX { get; set; }
        public FoxUInt32 BlockMarginZ { get; set; }
        public FoxUInt32 LoadingDistanceX { get; set; }
        public FoxUInt32 LoadingDistanceZ { get; set; }
        public FoxUInt32 CommonBlockSizeInBytes { get; set; }
        public FoxUInt32 LargeBlockCount0 { get; set; }
        public FoxUInt32 LargeBlockSizeInBytes0 { get; set; }
        public FoxUInt32 LargeBlockCount1 { get; set; }
        public FoxUInt32 LargeBlockSizeInBytes1 { get; set; }
        public FoxUInt32 LargeBlockCount2 { get; set; }
        public FoxUInt32 LargeBlockSizeInBytes2 { get; set; }
        public FoxUInt32 LargeBlockCount3 { get; set; }
        public FoxUInt32 LargeBlockSizeInBytes3 { get; set; }
        public FoxUInt32 LargeBlockLoadingMarginX { get; set; }
        public FoxUInt32 LargeBlockLoadingMarginZ { get; set; }
        public FoxFilePtr StageBlockFile { get; set; }
    }
}
