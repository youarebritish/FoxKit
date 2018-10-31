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
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Note: Gimmicks are weird. They're Data, not TransformData, and get their transform from an lba.
    /// </summary>
    [Serializable]
    public class TppPermanentGimmickData : Data
    {
        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private UnityEngine.Object partsFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private UnityEngine.Object locatorFile;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private TppPermanentGimmickParameter parameters;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private uint flags1 = 5;

        /// <summary>
        /// TODO Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Gimmick")]
        private uint flags2;

        /// <summary>
        /// Path to <see cref="partsFile"/>.
        /// </summary>
        [SerializeField]
        private string partsFilePath = string.Empty;

        /// <summary>
        /// Path to <see cref="locatorFile"/>.
        /// </summary>
        [SerializeField]
        private string locatorFilePath = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 144;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);
            tryGetAsset(this.partsFilePath, out this.partsFile);
            tryGetAsset(this.locatorFilePath, out this.locatorFile);
        }
    }
}