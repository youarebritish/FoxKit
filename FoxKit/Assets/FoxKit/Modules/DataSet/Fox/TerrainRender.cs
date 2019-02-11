namespace FoxKit.Modules.DataSet.Fox
{
    using FoxKit.Utils;

    public enum WolrdTerrainTextureMode : int
    {
        None,
        Default,
        New
    }

    public partial class TerrainRender
    {
        public TerrainRender() : base()
        {
            this.materials.Populate(16);
        }
    }
}
