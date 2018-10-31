namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

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
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 104)]
        private string attackId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 112)]
        private string equipId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 120)]
        private string bulletId;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt8, 176)]
        private byte weaponImplTypeIndex;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 160)]
        private float fireInterval;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 56)]
        private UnityEngine.Object weaponFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 80)]
        private UnityEngine.Object ammoFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 128)]
        private string ownerCnpName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 136)]
        private string weaponBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 144)]
        private string turretBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 152)]
        private string barrelBoneName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 164)]
        private float minPitch;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 168)]
        private float maxPitch = 1.5f;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 172)]
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
    }
}