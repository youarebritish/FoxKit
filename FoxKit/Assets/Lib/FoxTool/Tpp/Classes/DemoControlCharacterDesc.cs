using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DemoControlCharacterDesc
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString CharacterId { get; set; }
        public FoxString ReleaseGroupName { get; set; }
        public FoxString ReleaseTag { get; set; }
        public FoxBool ControlledAtStart { get; set; }
        public FoxVector3 Translation { get; set; }
        public FoxQuat Rotation { get; set; }
        public FoxString StartGroupName { get; set; }
        public FoxString StartTag { get; set; }
        public FoxVector3 StartTranslation { get; set; }
        public FoxQuat StartRotation { get; set; }
    }
}
