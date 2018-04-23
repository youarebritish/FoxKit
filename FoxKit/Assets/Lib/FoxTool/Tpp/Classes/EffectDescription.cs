using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class EffectDescription : Data
    {
        // Static properties
        public FoxEntityLink Depends { get; set; }
        public FoxString PartName { get; set; }
        public FoxString BuildType { get; set; }
        public FoxString ConnectDestinationSkelNames { get; set; }
        public FoxString ConnectDestinationCnpNames { get; set; }
        public FoxVector3 OffsetSkelPositions { get; set; }
        public FoxVector3 OffsetCnpPositions { get; set; }
        public FoxVector4 GeneralSkelParameters { get; set; }
        public FoxVector4 GeneralCnpParameters { get; set; }
        public FoxBool EffectConnect { get; set; }
        public FoxBool ChangeEffectConnectSetting { get; set; }
        public FoxBool VisibleModelWithEffect { get; set; }
        public FoxBool CreateStartEffect { get; set; }
        public FoxUInt32 EffectRandomSeed { get; set; }
        public FoxInt32 EffectKind { get; set; }
        public FoxFilePtr EffectFile { get; set; }
    }
}
