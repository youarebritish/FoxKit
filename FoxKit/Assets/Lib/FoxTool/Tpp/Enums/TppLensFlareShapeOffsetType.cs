using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    public class TppLensFlareShapeOffsetType : FoxSwitch
    {
        public static readonly TppLensFlareShapeOffsetType Instance =
            new TppLensFlareShapeOffsetType();

        public static readonly FoxEnumValue OffsetTypeNoMove = new FoxEnumValue("OFFSET_TYPE_NO_MOVE", 0);
        public static readonly FoxEnumValue OffsetTypeTogether = new FoxEnumValue("OFFSET_TYPE_TOGETHER", 1);
        public static readonly FoxEnumValue OffsetTypeMirror = new FoxEnumValue("OFFSET_TYPE_MIRROR", 2);
        public static readonly FoxEnumValue OffsetTypeXMirror = new FoxEnumValue("OFFSET_TYPE_X_MIRROR", 3);
        public static readonly FoxEnumValue OffsetTypeYMirror = new FoxEnumValue("OFFSET_TYPE_Y_MIRROR", 4);

        protected TppLensFlareShapeOffsetType()
        {
            _values.Add(OffsetTypeNoMove);
            _values.Add(OffsetTypeTogether);
            _values.Add(OffsetTypeMirror);
            _values.Add(OffsetTypeXMirror);
            _values.Add(OffsetTypeYMirror);
        }
    }
}
