using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppCoverPoint : Data
    {
        // Static properties
        public FoxEntityHandle Parent { get; set; }
        public FoxEntityPtr Transform { get; set; }
        public FoxEntityPtr ShearTransform { get; set; }
        public FoxEntityPtr PivotTransform { get; set; }
        public FoxEntityHandle Children { get; set; }
        public FoxUInt32 Flags { get; set; }
        public FoxUInt8 Tags { get; set; }
        public FoxEntityLink UserData { get; set; }
        public FoxBool IsLeftOpen { get; set; }
        public FoxBool IsRightOpen { get; set; }
        public FoxBool IsUpOpen { get; set; }
        public FoxBool IsStandable { get; set; }
        public FoxBool IsUnVaultable { get; set; }
        public FoxBool IsUseVip { get; set; }
    }
}
