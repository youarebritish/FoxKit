using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class AnimGraphLayerDescription : DataElement
    {
        public FoxEntityHandle Owner { get; set; }
        public FoxString LayerName { get; set; }
        public FoxPath GraphPath { get; set; }
        public FoxString MaskName { get; set; }
    }
}
