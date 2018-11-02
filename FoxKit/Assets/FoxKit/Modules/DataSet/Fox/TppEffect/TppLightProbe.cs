namespace FoxKit.Modules.DataSet.Fox.TppEffect
{
    public enum TppLightProbe_DebugMode : int
    {
        Off = 0,
        Cubemap = 1,
        SHValue = 2,
        SHOcclusion = 3,
        SHWithSky = 4,
        SHOnlyCol = 5,
        SHOnlySubCol = 6,
        SHOnlyLightAll = 7
    }

    public enum TppLightProbe_DrawRejectionLevel : int
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        NoReject = 7
    }

    public enum TppLightProbe_PackingGeneration : int
    {
        All = 0,
        Generation7 = 1,
        Generation8 = 2
    }

    public enum TppLightProbe_ShapeType : int
    {
        Default = 0,
        TrialgularPrism = 1,
        SemiCylindrical = 2,
        HalfSquare = 3
    }

    public partial class TppLightProbe
    {
    }
}
