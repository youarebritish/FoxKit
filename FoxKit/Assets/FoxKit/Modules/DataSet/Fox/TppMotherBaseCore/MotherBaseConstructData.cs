namespace FoxKit.Modules.DataSet.Fox.TppMotherBaseCore
{
    using FoxKit.Utils;

    public enum MbConstructDataType : int
    {
        PlantHq,
        PlantUnique,
        PlantCommon,
        Connector,
        Bridge,
        ClusterBridge,
        PrerequisitDummy
    }

    public partial class MotherBaseConstructData
    {
        public MotherBaseConstructData()
            : base()
        {
            this.divisionType.Populate(4);
            this.divisionRotate.Populate(4);
            this.anotherConnector.Populate(8);
        }
    }
}
