using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxKit.Modules.DataSet.Importer;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;
using System.Collections.Generic;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        public byte VehicleTypeCode;
        public byte AttachmentImplTypeIndex;
        public UnityEngine.Object AttachmentFile;
        public byte AttachmentInstanceCount;
        public string BodyCnpName;
        public string AttachmentBoneName;
        public List<TppVehicle2WeaponParameter> WeaponParams;

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
            else if (propertyData.Name == "attachmentFile")
            {
                var filePtr = DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData);
                var fileFound = DataSetUtils.TryGetFile(filePtr, out AttachmentFile);
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
            else if (propertyData.Name == "weaponParams")
            {
                var list = DataSetUtils.GetDynamicArrayValues<FoxEntityPtr>(propertyData);
                WeaponParams = new List<TppVehicle2WeaponParameter>(list.Count);

                foreach (var param in list)
                {
                    var entity = getEntity(param.EntityPtr) as TppVehicle2WeaponParameter;
                    WeaponParams.Add(entity);
                    entity.Owner = this;
                }
            }
        }
    }
}