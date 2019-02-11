namespace FoxKit.Modules.DataSet.Fox.UiScene
{
    using System;

    [Flags]
    public enum UiModelDataFlag : uint
    {
        DispOn = 1,
        ZTest = 2,
        GameCamera = 4,
        Billboard = 8,
        StencilModel = 16,
        StencilOut = 32,
        DrawStencilOff = 64,
        TextureNoWait = 128
    }

    [Flags]
    public enum UiInheritanceSetting : uint
    {
        Scale = 1,
        Rotation = 2,
        Translation = 4,
        ColorRGB = 8,
        ColorAlpha = 16
    }

    public partial class UiModelData
    {
    }
}
