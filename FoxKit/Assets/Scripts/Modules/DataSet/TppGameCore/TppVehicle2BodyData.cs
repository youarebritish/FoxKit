using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;
using System.Collections.Generic;

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
        public List<TppVehicle2WeaponParameter> WeaponParams;
        public List<UnityEngine.Object> FovaFiles;

        public string PartsFilePath;
        public List<string> FovaFilesPaths;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

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
                PartsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "bodyInstanceCount")
            {
                BodyInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "weaponParams")
            {
                var addresses = DataSetUtils.GetDynamicArrayValues<FoxEntityPtr>(propertyData);
                WeaponParams = new List<TppVehicle2WeaponParameter>(addresses.Count);

                foreach(var address in addresses)
                {
                    var param = initFunctions.GetEntityFromAddress(address.EntityPtr) as TppVehicle2WeaponParameter;
                    WeaponParams.Add(param);
                    param.Owner = this;
                }
            }
            else if (propertyData.Name == "fovaFiles")
            {
                var filePtrList = DataSetUtils.GetDynamicArrayValues<FoxFilePtr>(propertyData);
                FovaFiles = new List<UnityEngine.Object>(filePtrList.Count);
                FovaFilesPaths = new List<string>(filePtrList.Count);

                foreach (var filePtr in filePtrList)
                {
                    var path = DataSetUtils.ExtractFilePath(filePtr);
                    FovaFilesPaths.Add(path);
                }
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetImportedAsset)
        {
            base.OnAssetsImported(tryGetImportedAsset);
            tryGetImportedAsset(PartsFilePath, out PartsFile);

            foreach(var fovaFilePath in FovaFilesPaths)
            {
                UnityEngine.Object file = null;
                tryGetImportedAsset(fovaFilePath, out file);
                FovaFiles.Add(file);
            }
        }
    }
}