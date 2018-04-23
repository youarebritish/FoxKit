using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    public class TppLensFlareFieldShapeType : FoxSwitch
    {
        public static readonly TppLensFlareFieldShapeType Instance =
            new TppLensFlareFieldShapeType();

        public static readonly FoxEnumValue ShapeTypeSquare = new FoxEnumValue("SHAPE_TYPE_SQUARE", 0);
        public static readonly FoxEnumValue ShapeTypeCircle = new FoxEnumValue("SHAPE_TYPE_CIRCLE", 1);

        protected TppLensFlareFieldShapeType()
        {
            _values.Add(ShapeTypeSquare);
            _values.Add(ShapeTypeCircle);
        }
    }
}
