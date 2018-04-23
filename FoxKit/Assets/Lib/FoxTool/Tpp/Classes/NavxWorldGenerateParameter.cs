using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class NavxWorldGenerateParameter : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxFloat Resolution { get; set; }
        public FoxFloat VerticalThreshold { get; set; }
        public FoxPath RoughGraphFilePath { get; set; }
        public FoxFilePtr RoughGraphFilePtr { get; set; }
        public FoxString WorldName { get; set; }
        public FoxUInt32 MaxFileSizeInKb { get; set; }
        public List<FoxEntityPtr> Parameters { get; set; }
        public FoxUInt32 SectorSizeHorizontal { get; set; }
        public FoxUInt32 TileSizeHorizontal { get; set; }
        public FoxUInt32 SearchSpaceBucketSizeHorizontal { get; set; }
        public List<FoxString> CollisionAttributes { get; set; }
    }
}
