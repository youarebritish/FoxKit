namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

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
        public override short ClassId => 128;

        /// <inheritdoc />
        public override ushort Version => 3;

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
    }
}