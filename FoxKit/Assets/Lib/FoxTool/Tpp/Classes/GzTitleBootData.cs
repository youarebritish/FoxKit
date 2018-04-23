using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class GzTitleBootData : Data
    {
        // Static properties
        public FoxFilePtr UigFiles { get; set; }
        public List<FoxFilePtr> RawFiles { get; set; }
    }
}
