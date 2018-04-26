using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxKit.Modules.DataSet.Importer;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        public byte VehicleTypeCode;
        public byte AttachmentImplTypeIndex;
        public UnityEngine.Object AttachmentFile;   // TODO
        public byte AttachmentInstanceCount;
        public string BodyCnpName;
        public string AttachmentBoneName;

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "vehicleTypeCode")
            {
                VehicleTypeCode = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "attachmentImplTypeIndex")
            {
                AttachmentImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "attachmentInstanceCount")
            {
                AttachmentInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "bodyCnpName")
            {
                BodyCnpName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "attachmentBoneName")
            {
                AttachmentBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}