namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

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
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private byte vehicleTypeCode;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private byte attachmentImplTypeIndex;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private UnityEngine.Object attachmentFile;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private byte attachmentInstanceCount;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private string bodyCnpName;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private string attachmentBoneName;

        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private List<TppVehicle2WeaponParameter> weaponParams = new List<TppVehicle2WeaponParameter>();

        /// <summary>
        /// Path to <see cref="attachmentFile"/>.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Attachment")]
        private string attachmentFilePath;

        /// <inheritdoc />
        public override short ClassId => 120;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.attachmentFilePath, out this.attachmentFile);
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "vehicleTypeCode":
                    this.vehicleTypeCode = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "attachmentImplTypeIndex":
                    this.attachmentImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "attachmentFile":
                    this.attachmentFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "attachmentInstanceCount":
                    this.attachmentInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "bodyCnpName":
                    this.bodyCnpName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "attachmentBoneName":
                    this.attachmentBoneName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "weaponParams":
                    var list = DataSetUtils.GetDynamicArrayValues<ulong>(propertyData);
                    this.weaponParams = new List<TppVehicle2WeaponParameter>(list.Count);

                    foreach (var param in list)
                    {
                        var entity = initFunctions.GetEntityFromAddress(param) as TppVehicle2WeaponParameter;
                        Assert.IsNotNull(entity);

                        this.weaponParams.Add(entity);
                        entity.Owner = this;
                    }

                    break;
            }
        }
    }
}