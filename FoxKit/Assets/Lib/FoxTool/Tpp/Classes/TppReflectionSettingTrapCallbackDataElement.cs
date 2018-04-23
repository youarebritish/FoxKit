using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppReflectionSettingTrapCallbackDataElement
    {
        // Static properties
        public FoxEntityHandle Owner { get; set; }
        public FoxString FuncName { get; set; }
        public FoxPath ReflectionTexturePath { get; set; }
        public FoxPath ReflectionTexturePathForGoOut { get; set; }
    }
}
