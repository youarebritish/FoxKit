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
    /// TODO: Figure out.
    /// </summary>
    [Serializable]
    public class TppVehicle2BodyData : Data
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private byte vehicleTypeIndex;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private byte proxyVehicleTypeIndex;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private byte bodyImplTypeIndex;

        /// <summary>
        /// PartsFile defining the vehicle's model data.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private UnityEngine.Object partsFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private byte bodyInstanceCount = 2;

        /// <summary>
        /// Weapon parameters.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private List<TppVehicle2WeaponParameter> weaponParams = new List<TppVehicle2WeaponParameter>();

        /// <summary>
        /// Form variation files.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Vehicle Body")]
        private List<UnityEngine.Object> fovaFiles = new List<UnityEngine.Object>();

        /// <summary>
        /// Path to <see cref="partsFile"/>.
        /// </summary>
        [SerializeField, HideInInspector]
        private string partsFilePath;

        /// <summary>
        /// Paths to <see cref="fovaFiles"/>.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<string> fovaFilesPaths;

        /// <inheritdoc />
        protected override short ClassId => 128;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.partsFilePath, out this.partsFile);

            foreach (var fovaFilePath in this.fovaFilesPaths)
            {
                UnityEngine.Object file;
                tryGetAsset(fovaFilePath, out file);
                this.fovaFiles.Add(file);
            }
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "vehicleTypeIndex":
                    this.vehicleTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "proxyVehicleTypeIndex":
                    this.proxyVehicleTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "bodyImplTypeIndex":
                    this.bodyImplTypeIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "partsFile":
                    this.partsFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "bodyInstanceCount":
                    this.bodyInstanceCount = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
                case "weaponParams":
                    var addresses = DataSetUtils.GetDynamicArrayValues<ulong>(propertyData);
                    this.weaponParams = new List<TppVehicle2WeaponParameter>(addresses.Count);

                    foreach (var address in addresses)
                    {
                        var param = initFunctions.GetEntityFromAddress(address) as TppVehicle2WeaponParameter;
                        Assert.IsNotNull(param, $"Parameter in {this.name} must not be null.");

                        this.weaponParams.Add(param);
                        param.Owner = this;
                    }

                    break;
                case "fovaFiles":
                    var filePtrList = DataSetUtils.GetDynamicArrayValues<string>(propertyData);
                    this.fovaFiles = new List<UnityEngine.Object>(filePtrList.Count);
                    this.fovaFilesPaths = new List<string>(filePtrList.Count);

                    foreach (var filePtr in filePtrList)
                    {
                        var path = DataSetUtils.ExtractFilePath(filePtr);
                        this.fovaFilesPaths.Add(path);
                    }

                    break;
            }
        }
    }
}