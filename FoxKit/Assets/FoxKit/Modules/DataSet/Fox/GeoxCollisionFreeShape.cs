namespace FoxKit.Modules.DataSet.Fox
{
    using FoxKit.Utils;

    public enum CollisionFreeShape_Category : int
    {
        Alll = 0,
        Chara = 1,
        Recoil = 2
    }

    public partial class GeoxCollisionFreeShape
    {
        public GeoxCollisionFreeShape() : base()
        {
            this.points.Populate(8);
        }
    }
}
