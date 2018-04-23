using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class PhBallsocketConstraintParam
    {
        // Static properties
        public FoxVector3 DefaultPosition { get; set; }
        public FoxBool LimitedFlag { get; set; }
        public FoxVector3 RefA { get; set; }
        public FoxVector3 RefB { get; set; }
        public FoxFloat Limit { get; set; }
        public FoxBool SpringFlag { get; set; }
        public FoxBool SpringRefCustomFlag { get; set; }
        public FoxVector3 SpringRef { get; set; }
        public FoxFloat SpringConstant { get; set; }
        public FoxFloat Flexibility { get; set; }
        public FoxBool StopTwistFlag { get; set; }
    }
}
