namespace FoxKit.Modules.DataSet.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// TODO: Figure out.
    /// </summary>
    [Serializable]
    public class UiInstanceData : Data
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("UI")]
        private OrderedDictionary_string_Object createWindowParams = new OrderedDictionary_string_Object();

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("UI")]
        private string windowFactoryName = string.Empty;

        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField, HideInInspector]
        private OrderedDictionary_string_string createWindowParamsPaths = new OrderedDictionary_string_string();

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Canvas)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 120;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            foreach (var path in this.createWindowParamsPaths)
            {
                UnityEngine.Object file;
                tryGetAsset(path.Value, out file);
                this.createWindowParams.Add(path.Key, file);
            }
        }
    }
}