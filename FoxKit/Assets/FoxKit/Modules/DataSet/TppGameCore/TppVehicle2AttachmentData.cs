namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure this out.
    /// </summary>
    [Serializable]
    public class TppVehicle2AttachmentData : Data
    {
        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private byte vehicleTypeCode;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private byte attachmentImplTypeIndex;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private UnityEngine.Object attachmentFile;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private byte attachmentInstanceCount;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private string bodyCnpName;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private string attachmentBoneName;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private List<TppVehicle2WeaponParameter> weaponParams = new List<TppVehicle2WeaponParameter>();

        /// <summary>
        /// Path to <see cref="attachmentFile"/>.
        /// </summary>
        [SerializeField, Category("Vehicle Attachment")]
        private string attachmentFilePath;

        /// <inheritdoc />
        protected override short ClassId => 120;

        /// <inheritdoc />
        public override void OnAssetsImported(Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.attachmentFilePath, out this.attachmentFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "vehicleTypeCode":
                    this.vehicleTypeCode = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
                    break;
                case "attachmentImplTypeIndex":
                    this.attachmentImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
                    break;
                case "attachmentFile":
                    this.attachmentFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<FoxFilePtr>(propertyData));
                    break;
                case "attachmentInstanceCount":
                    this.attachmentInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt8>(propertyData).Value;
                    break;
                case "bodyCnpName":
                    this.bodyCnpName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "attachmentBoneName":
                    this.attachmentBoneName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "weaponParams":
                    var list = DataSetUtils.GetDynamicArrayValues<FoxEntityPtr>(propertyData);
                    this.weaponParams = new List<TppVehicle2WeaponParameter>(list.Count);

                    foreach (var param in list)
                    {
                        var entity = initFunctions.GetEntityFromAddress(param.EntityPtr) as TppVehicle2WeaponParameter;
                        Assert.IsNotNull(entity);

                        this.weaponParams.Add(entity);
                        entity.Owner = this;
                    }

                    break;
            }
        }
    }
}