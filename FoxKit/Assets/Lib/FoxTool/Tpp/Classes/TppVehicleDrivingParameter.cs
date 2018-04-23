using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppVehicleDrivingParameter : Data
    {
        // Static properties
        public FoxFloat PidSpeedKp { get; set; }
        public FoxFloat PidSpeedTi { get; set; }
        public FoxFloat PidSpeedTd { get; set; }
        public FoxFloat PidSpeedEta { get; set; }
        public FoxFloat PidSpeedDt { get; set; }
        public FoxFloat PidSteeringKp { get; set; }
        public FoxFloat PidSteeringTi { get; set; }
        public FoxFloat PidSteeringTd { get; set; }
        public FoxFloat PidSteeringEta { get; set; }
        public FoxFloat PidSteeringDt { get; set; }
        public FoxFloat SteeringPredictFutureTime { get; set; }
        public FoxVector3 SteeringControleOffset { get; set; }
        public FoxFloat AdjustOffset { get; set; }
        public FoxFloat SteeringMargin { get; set; }
        public FoxFloat MinimumTurningRadius { get; set; }
    }
}
