namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppSharedGimmickData : Data
    {
        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _modelFile;

        [SerializeField, HideInInspector]
        private string modelFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _geomFile;

        [SerializeField, HideInInspector]
        private string geomFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _breakedModelFile;

        [SerializeField, HideInInspector]
        private string breakedModelFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _breakedGeomFile;

        [SerializeField, HideInInspector]
        private string breakedGeomFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _partsFile;

        [SerializeField, HideInInspector]
        private string partsFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private uint _numDynamicGimmick;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private UnityEngine.Object _locaterFile;

        [SerializeField, HideInInspector]
        private string locaterFilePath;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private uint _flags1;

        [SerializeField, Modules.DataSet.Property("TppSharedGimmickData")]
        private uint _flags2;

        /// <inheritdoc />
        public override short ClassId => 240;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.modelFilePath, out this._modelFile);
            tryGetAsset(this.geomFilePath, out this._geomFile);
            tryGetAsset(this.breakedModelFilePath, out this._breakedModelFile);
            tryGetAsset(this.breakedGeomFilePath, out this._breakedGeomFile);
            tryGetAsset(this.partsFilePath, out this._partsFile);
            tryGetAsset(this.locaterFilePath, out this._locaterFile);
        }
    }
}
