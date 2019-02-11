namespace FoxKit.Modules.DataSet.Fox.Ui
{
    public enum UiNodeType : int
    {
        Page = 0,
        Phase = 1,
        Event = 2,
        Action = 4,
        Unknown = 5,
        Compound = 6
    }

    public enum UiNodePropType : int
    {
        Int8 = 0,
        UInt8 = 1,
        Int16 = 2,
        UInt16 = 3,
        Int32 = 4,
        UInt32 = 5,
        Int64 = 6,
        UInt64 = 7,
        Float32 = 8,
        Float64 = 9,
        Bool = 10,
        String = 11,
        Path = 12,
        EntityPtr = 13,
        Vector3 = 14,
        Vector4 = 15,
        Quat = 16,
        Matrix3 = 17,
        Matrix4 = 18,
        Color = 19,
        FilePtr = 20,
        EntityHandle = 21,
        EntityLink = 22,
        Invalid = 23
    }

    public partial class UiNodeData
    {
    }
}
