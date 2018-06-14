namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="TppVehicle2AttachmentData"/> Entity.
    /// </summary>
    [Serializable]
    public class TppVehicle2WeaponParameter : DataElement<Data>
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
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.weaponFilePath, out this.weaponFile);
            tryGetAsset(this.ammoFilePath, out this.ammoFile);
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