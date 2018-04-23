using System.Collections.Generic;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DataSet : Data
    {
        public Dictionary<string, FoxEntityPtr> DataList { get; set; }
    }
}
