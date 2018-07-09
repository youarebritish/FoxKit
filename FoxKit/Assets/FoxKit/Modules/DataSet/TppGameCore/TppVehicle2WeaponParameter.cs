namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="TppVehicle2AttachmentData"/> Entity.
    /// </summary>
    [Serializable]
    public class TppVehicle2WeaponParameter : DataElement//<Data>
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string attackId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string equipId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string bulletId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private byte weaponImplTypeIndex;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float fireInterval;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object weaponFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object ammoFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string ownerCnpName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string weaponBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string turretBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string barrelBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float minPitch;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float maxPitch = 1.5f;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotSpeed = 0.5f;

        /// <summary>
        /// Path to <see cref="weaponFile"/>.
        /// </summary>
        [SerializeField]
        private string weaponFilePath;

        /// <summary>
        /// Path to <see cref="ammoFile"/>.
        /// </summary>
        [SerializeField]
        private string ammoFilePath;

        /// <inheritdoc />
        public override short ClassId => 128;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.weaponFilePath, out this.weaponFile);
            tryGetAsset(this.ammoFilePath, out this.ammoFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("attackId", Core.PropertyInfoType.String, this.attackId));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("equipId", Core.PropertyInfoType.String, this.equipId));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("bulletId", Core.PropertyInfoType.String, this.bulletId));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("weaponImplTypeIndex", Core.PropertyInfoType.UInt8, this.weaponImplTypeIndex));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("fireInterval", Core.PropertyInfoType.Float, this.fireInterval));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("weaponFile", Core.PropertyInfoType.FilePtr, DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.weaponFile))));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ammoFile", Core.PropertyInfoType.FilePtr, DataSetUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(this.ammoFile))));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ownerCnpName", Core.PropertyInfoType.String, this.ownerCnpName));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("weaponBoneName", Core.PropertyInfoType.String, this.weaponBoneName));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("turretBoneName", Core.PropertyInfoType.String, this.turretBoneName));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("barrelBoneName", Core.PropertyInfoType.String, this.barrelBoneName));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("minPitch", Core.PropertyInfoType.Float, this.minPitch));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("maxPitch", Core.PropertyInfoType.Float, this.maxPitch));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("rotSpeed", Core.PropertyInfoType.Float, this.rotSpeed));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "attackId":
                    this.attackId = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "equipId":
                    this.equipId = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "bulletId":
                    this.bulletId = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "weaponImplTypeIndex":
                    this.weaponImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "fireInterval":
                    this.fireInterval = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "weaponFile":
                    this.weaponFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "ammoFile":
                    this.ammoFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "weaponBoneName":
                    this.weaponBoneName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "turretBoneName":
                    this.turretBoneName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "barrelBoneName":
                    this.barrelBoneName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "minPitch":
                    this.minPitch = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "maxPitch":
                    this.maxPitch = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "rotSpeed":
                    this.rotSpeed = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
            }
        }
    }
}