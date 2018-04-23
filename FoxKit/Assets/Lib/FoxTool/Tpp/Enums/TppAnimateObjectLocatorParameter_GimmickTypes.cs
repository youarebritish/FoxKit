using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    //name=TppAnimateObjectLocatorParameter_GimmickTypes
    public class TppAnimateObjectLocatorParameterGimmickTypes : FoxSwitch
    {
        public static readonly TppAnimateObjectLocatorParameterGimmickTypes Instance =
            new TppAnimateObjectLocatorParameterGimmickTypes();

        public static readonly FoxEnumValue TypeAnimateDefault = new FoxEnumValue("TYPE_ANIMATE_DEFAULT", 0);
        public static readonly FoxEnumValue TypeAnimatedBreakable = new FoxEnumValue("TYPE_ANIMATED_BREAKABLE", 1);
        public static readonly FoxEnumValue TypeCurtain = new FoxEnumValue("TYPE_CURTAIN", 2);

        protected TppAnimateObjectLocatorParameterGimmickTypes()
        {
            _values.Add(TypeAnimateDefault);
            _values.Add(TypeAnimatedBreakable);
            _values.Add(TypeCurtain);
        }
    }
}
