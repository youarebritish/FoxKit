using FoxTool.Fox.Enums;

namespace FoxTool.Tpp.Enums
{
    public class TppLensFlareShapeDistanceScalingMode : FoxSwitch
    {
        public static readonly TppLensFlareShapeDistanceScalingMode Instance =
            new TppLensFlareShapeDistanceScalingMode();

        public static readonly FoxEnumValue DistanceScalingModeNone = new FoxEnumValue("DISTANCE_SCALING_MODE_NONE", 0);

        public static readonly FoxEnumValue DistanceScalingModeSizescale =
            new FoxEnumValue("DISTANCE_SCALING_MODE_SIZESCALE", 1);

        public static readonly FoxEnumValue DistanceScalingModeAlphascale =
            new FoxEnumValue("DISTANCE_SCALING_MODE_ALPHASCALE", 2);

        public static readonly FoxEnumValue DistanceScalingModeSizescale2 =
            new FoxEnumValue("DISTANCE_SCALING_MODE_SIZESCALE2", 3);

        protected TppLensFlareShapeDistanceScalingMode()
        {
            _values.Add(DistanceScalingModeNone);
            _values.Add(DistanceScalingModeSizescale);
            _values.Add(DistanceScalingModeAlphascale);
            _values.Add(DistanceScalingModeSizescale2);
        }
    }
}
