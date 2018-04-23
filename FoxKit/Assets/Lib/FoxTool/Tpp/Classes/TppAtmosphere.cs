using FoxTool.Fox.Types.Structs;
using FoxTool.Fox.Types.Values;

namespace FoxTool.Tpp.Classes
{
    public class TppAtmosphere : Data
    {
        // Static properties
        public FoxPath FilePath { get; set; }
        public FoxFilePtr FilePtr { get; set; }
        public FoxEntityHandle CapturePosition { get; set; }
        public FoxFloat RayleighHeightScale { get; set; }
        public FoxVector3 RayleighScatteringCoefficient { get; set; }
        public FoxFloat RayleighHeightScaleOfCloudySky { get; set; }
        public FoxVector3 RayleighScatteringCoefficientOfCloudySky { get; set; }
        public FoxFloat MieHeightScale { get; set; }
        public FoxFloat MieAbsorptionRate { get; set; }
        public FoxFloat MieAnisotropy { get; set; }
        public FoxVector3 MieScatteringCoefficient { get; set; }
        public FoxFloat MieHeightScaleOfCloudySky { get; set; }
        public FoxFloat MieAbsorptionRateOfCloudySky { get; set; }
        public FoxFloat MieAnisotropyOfCloudySky { get; set; }
        public FoxVector3 MieScatteringCoefficientOfCloudySky { get; set; }
        public FoxUInt32 MultiScatteringOrder { get; set; }
        public FoxColor GroundColor { get; set; }
        public FoxFloat NorthAngle { get; set; }
        public FoxFloat Latitude { get; set; }
        public FoxFloat Longitude { get; set; }
        public FoxFloat Altitude { get; set; }
        public FoxInt32 GmtTimeDifference { get; set; }
        public FoxUInt32 Year { get; set; }
        public FoxUInt32 Month { get; set; }
        public FoxUInt32 Day { get; set; }
        public FoxBool SkyEnable { get; set; }
        public FoxBool SunLightEnable { get; set; }
        public FoxFloat ShadowRange { get; set; }
        public FoxFloat ShadowRangeExtra { get; set; }
        public FoxFloat HiResShadowRange { get; set; }
        public FoxFloat ShadowProjectionRange { get; set; }
        public FoxFloat ShadowFadeRange { get; set; }
        public FoxFloat SelfShadowBias { get; set; }
        public FoxBool IsCascadeBlend { get; set; }
        public FoxBool CastShadow { get; set; }
        public FoxFloat ShadowMaskSpecular { get; set; }
        public FoxFloat ShadowOffsetStartAngle { get; set; }
        public FoxFloat ShadowOffsetEndAngle { get; set; }
        public FoxFloat SunLux { get; set; }
        public FoxFloat MoonLux { get; set; }
        public FoxFloat StarLight { get; set; }
        public FoxColor MoonColor { get; set; }
        public FoxVector3 FixedMoonDir { get; set; }
        public FoxInt32 MoonMovement { get; set; }
        public FoxFloat SkyLightSunScale { get; set; }
        public FoxFloat SkyLightMoonScale { get; set; }
        public FoxFloat SkyColorSunScale { get; set; }
        public FoxFloat SkyColorMoonScale { get; set; }
        public FoxFloat DaySkyAmbientScale { get; set; }
        public FoxFloat NightSkyAmbientScale { get; set; }
        public FoxFloat DirLightLimitAngle { get; set; }
        public FoxFloat DirLightAttenuStart { get; set; }
        public FoxFloat DirLightAttenuEnd { get; set; }
        public FoxVector3 FixedLightDirSunRise { get; set; }
        public FoxVector3 FixedLightDirSunSet { get; set; }
        public FoxVector3 FixedLightDirMoonRise { get; set; }
        public FoxVector3 FixedLightDirMoonSet { get; set; }
        public FoxBool DisableSkyCapture { get; set; }
        public FoxBool SkyLightEnable { get; set; }
        public FoxBool UsePrecomputedAmbient { get; set; }
        public FoxFloat SkyLightLuminanceScale { get; set; }
        public FoxUInt32 NumBands { get; set; }
        public FoxVector4 Coefficients { get; set; }
        public FoxFloat Cloudiness { get; set; }
        public FoxUInt32 LocalFlags { get; set; }
    }
}
