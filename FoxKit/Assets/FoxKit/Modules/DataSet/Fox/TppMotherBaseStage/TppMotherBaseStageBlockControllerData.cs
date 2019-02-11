namespace FoxKit.Modules.DataSet.Fox.TppMotherBaseStage
{
    using FoxKit.Utils;

    public enum MbStageBlockCreateMode : int
    {
        NotCreate = 0,
        Create = 1
    }

    public partial class TppMotherBaseStageBlockControllerData
    {
        public TppMotherBaseStageBlockControllerData()
            : base()
        {
            this.cluster00divisionPackageIds.Populate(12);
            this.cluster01divisionPackageIds.Populate(12);
            this.cluster02divisionPackageIds.Populate(12);
            this.cluster03divisionPackageIds.Populate(12);
            this.cluster04divisionPackageIds.Populate(12);
            this.cluster05divisionPackageIds.Populate(12);
            this.cluster06divisionPackageIds.Populate(12);
            this.cluster07divisionPackageIds.Populate(12);

            this.clusterPositions.Populate(8);
            this.clusterRequestRadiuses.Populate(8);
            this.clusterRequireRadiuses.Populate(8);
        }
    }
}
