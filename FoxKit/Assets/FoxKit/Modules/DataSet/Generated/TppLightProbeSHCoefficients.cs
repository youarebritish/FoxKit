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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLightProbeSHCoefficients : Data
    {
        [SerializeField, Modules.DataSet.Property("TppLightProbeSHCoefficients")]
        private string _filePath = string.Empty;

        [SerializeField, HideInInspector]
        private string filePathPath;

        [SerializeField, Modules.DataSet.Property("TppLightProbeSHCoefficients")]
        private UnityEngine.Object _lpshFile;

        [SerializeField, HideInInspector]
        private string lpshFilePath;

        /// <inheritdoc />
        public override short ClassId => 104;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.lpshFilePath, out this._lpshFile);
        }
    }
}
