namespace FoxKit.Modules.DataSet.TppGameKit
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
    /// Mortar parameters for <see cref="TppPermanentGimmickData"/>.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickMortarParameter : TppPermanentGimmickParameter
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotationLimitLeftRight;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotationLimitUp;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private float rotationLimitDown;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object defaultShellPartsFile;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private UnityEngine.Object flareShellPartsFile;

        /// <summary>
        /// Path to <see cref="defaultShellPartsFile"/>.
        /// </summary>
        [SerializeField]
        private string defaultShellPartsFilePath;

        /// <summary>
        /// Path to <see cref="flareShellPartsFile"/>.
        /// </summary>
        [SerializeField]
        private string flareShellPartsFilePath;

        /// <inheritdoc />
        public override short ClassId => 88;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.defaultShellPartsFilePath, out this.defaultShellPartsFile);
            tryGetAsset(this.flareShellPartsFilePath, out this.flareShellPartsFile);
        }
    }
}