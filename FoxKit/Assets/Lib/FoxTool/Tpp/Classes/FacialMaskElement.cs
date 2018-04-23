using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class FacialMaskElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public List<FoxString> SkelList { get; set; }
        public List<FoxEntityPtr> ShaderList { get; set; }
    }
}
