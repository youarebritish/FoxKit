using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class Data : Entity
    {
        // Static properties
        public FoxString Name { get; set; }
        public FoxEntityHandle DataSet { get; set; }
    }
}
