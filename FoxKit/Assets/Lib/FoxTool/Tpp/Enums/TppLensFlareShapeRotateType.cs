using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    public class TppLensFlareShapeRotateType : FoxSwitch
    {
        public static readonly TppLensFlareShapeRotateType Instance =
            new TppLensFlareShapeRotateType();

        public static readonly FoxEnumValue RotateTypeNoRotate = new FoxEnumValue("ROTATE_TYPE_NO_ROTATE", 0);
        public static readonly FoxEnumValue RotateTypeSame = new FoxEnumValue("ROTATE_TYPE_SAME", 1);
        public static readonly FoxEnumValue RotateTypeReverse = new FoxEnumValue("ROTATE_TYPE_REVERSE", 2);

        protected TppLensFlareShapeRotateType()
        {
            _values.Add(RotateTypeNoRotate);
            _values.Add(RotateTypeSame);
            _values.Add(RotateTypeReverse);
        }
    }
}
