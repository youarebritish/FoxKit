namespace FoxKit.Modules.DataSet.Fox.Ui
{
    public enum UiOperationNodeKind : int
    {
        None = 0,
        And = 1,
        Or = 2,
        GreaterThan = 3,
        Switch = 4,
        PushFloat = 5,
        PushText = 6,
        PushColor = 7,
        PushVector3 = 8,
        OpePushArray = 9,
        OpePushResource = 10,
        OpeGetProperty = 11,
        OpeGetWindowName = 12,
        OpeCompare = 13,
        OpeTriggerParamToString = 14
    }

    public partial class UiOperationNode
    {
    }
}
