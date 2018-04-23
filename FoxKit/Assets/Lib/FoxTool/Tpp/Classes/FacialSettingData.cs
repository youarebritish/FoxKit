using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class FacialSettingData : Data
    {
        // Static properties
        public List<FoxEntityPtr> AspectMaskList { get; set; }
        public FoxEntityPtr MouthMask { get; set; }
        public FoxEntityPtr LipMask { get; set; }
        public FoxString RootName { get; set; }
    }
}
