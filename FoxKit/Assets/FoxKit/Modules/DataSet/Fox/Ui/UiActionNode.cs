namespace FoxKit.Modules.DataSet.Fox.Ui
{
    public enum UiActionNodeKind : int
    {
        None = 0,
        Display = 1,
        Animation = 2,
        Transform = 3,
        Color = 4,
        Texture = 5,
        FadeScene = 6,
        PageChange = 7,
        PhaseChange = 8,
        Script = 9,
        ChainList = 10,
        Sound = 11,
        SendTrigger = 12,
        SetText = 13,
        ColorGroup = 14,
        Priority = 15,
        ActAddWindow = 16,
        ActRemoveWindow = 17,
        ConnectComponent = 18
    }

    public partial class UiActionNode
    {
    }
}
