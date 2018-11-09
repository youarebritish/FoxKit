namespace FoxKit.Modules.DataSet.Fox.Grx
{
    using FoxKit.Utils;

    public enum OccluderEx_Mode : int
    {
        Global = 0,
        Local = 1
    }

    public partial class OccluderEx
    {
        public OccluderEx() : base()
        {
            this.positions.Populate(7);
        }
    }
}
