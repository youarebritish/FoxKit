using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class DebugFlyCameraLocator : Locator
    {
        public FoxBool Enable { get; set; }
        public FoxFloat Speed { get; set; }
        public FoxBool ForceUpdateCurrentPosition { get; set; }
    }
}
