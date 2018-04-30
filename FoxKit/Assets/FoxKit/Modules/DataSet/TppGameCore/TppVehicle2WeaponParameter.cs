using FoxKit.Modules.DataSet.FoxCore;
using System;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    [Serializable]
    public class TppVehicle2WeaponParameter : DataElement<Data>
    {
        public string AttackId;
        public string EquipId;
        public string BulletId;
        public byte WeaponImplTypeIndex;
        public float FireInterval;
        public UnityEngine.Object WeaponFile;
        public UnityEngine.Object AmmoFile;
        public string OwnerCnpName;
        public string WeaponBoneName;
        public string TurretBoneName;
        public float MinPitch;
        public float MaxPitch;
        public float RotSpeed;

        public string WeaponFilePath;
        public string AmmoFilePath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "attackId")
            {
                AttackId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "equipId")
            {
                EquipId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "bulletId")
            {
                BulletId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "weaponImplTypeIndex")
            {
                WeaponImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
            }
            else if (propertyData.Name == "fireInterval")
            {
                FireInterval = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "fireInterval")
            {
                FireInterval = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "weaponFile")
            {
                WeaponFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "ammoFile")
            {
                AmmoFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
            }
            else if (propertyData.Name == "weaponBoneName")
            {
                WeaponBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "turretBoneName")
            {
                TurretBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "minPitch")
            {
                MinPitch = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "maxPitch")
            {
                MaxPitch = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
            else if (propertyData.Name == "rotSpeed")
            {
                RotSpeed = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
        }

        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(WeaponFilePath, out WeaponFile);
            tryGetAsset(AmmoFilePath, out AmmoFile);
        }
    }
}