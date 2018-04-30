namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

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
        protected override short ClassId => 128;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.weaponFilePath, out this.weaponFile);
            tryGetAsset(this.ammoFilePath, out this.ammoFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "attackId":
                    this.attackId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "equipId":
                    this.equipId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "bulletId":
                    this.bulletId = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "weaponImplTypeIndex":
                    this.weaponImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
                    break;
                case "fireInterval":
                    this.fireInterval = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "weaponFile":
                    this.weaponFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "ammoFile":
                    this.ammoFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "weaponBoneName":
                    this.weaponBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "turretBoneName":
                    this.turretBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "minPitch":
                    this.minPitch = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "maxPitch":
                    this.maxPitch = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
                case "rotSpeed":
                    this.rotSpeed = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
                    break;
            }
        }
    }
}