using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class ChDamageTranslationRuleData : Data
    {
        public FoxString Key { get; set; }
        public FoxString DamageName { get; set; }
        public FoxPath ConditionScriptPath { get; set; }
        public FoxString TranslateScriptPath { get; set; }
    }
}
