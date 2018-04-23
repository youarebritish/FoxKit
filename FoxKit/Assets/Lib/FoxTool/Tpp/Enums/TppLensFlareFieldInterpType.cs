using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    public class TppLensFlareFieldInterpType : FoxSwitch
    {
        public static readonly TppLensFlareFieldInterpType Instance =
            new TppLensFlareFieldInterpType();

        public static readonly FoxEnumValue InterpTypeLinear = new FoxEnumValue("INTERP_TYPE_LINEAR", 0);
        public static readonly FoxEnumValue InterpTypeCos = new FoxEnumValue("INTERP_TYPE_COS", 1);
        public static readonly FoxEnumValue InterpTypeSudden = new FoxEnumValue("INTERP_TYPE_SUDDEN", 2);
        public static readonly FoxEnumValue InterpTypeBowl = new FoxEnumValue("INTERP_TYPE_BOWL", 3);

        protected TppLensFlareFieldInterpType()
        {
            _values.Add(InterpTypeLinear);
            _values.Add(InterpTypeCos);
            _values.Add(InterpTypeSudden);
            _values.Add(InterpTypeBowl);
        }
    }
}
