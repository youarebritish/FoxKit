using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;
using System.Collections.Generic;
using UnityEngine;
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2BodyData : Data
    {
        public byte VehicleTypeIndex;
        public byte ProxyVehicleTypeIndex;
        public byte BodyImplTypeIndex;
        public UnityEngine.Object PartsFile;
        public byte BodyInstanceCount;        
        public TppVehicle2WeaponParameter WeaponParams;
        public List<UnityEngine.Object> FovaFiles;

        protected override void ReadProperty(FoxProperty propertyData, GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "vehicleTypeIndex")
            {
                VehicleTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "proxyVehicleTypeIndex")
            {
                ProxyVehicleTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "bodyImplTypeIndex")
            {
                BodyImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "partsFile")
            {
                // TODO
            }
            else if (propertyData.Name == "bodyInstanceCount")
            {
                BodyInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "weaponParams")
            {
                // DynamicArray
                //var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                //WeaponParams = getEntity(address) as TppVehicle2WeaponParameter;
            }
            else if (propertyData.Name == "fovaFiles")
            {
                // TODO
            }
        }
    }
}