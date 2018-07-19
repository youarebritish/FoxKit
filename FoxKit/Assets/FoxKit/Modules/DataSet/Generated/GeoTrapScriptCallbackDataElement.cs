namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using NUnit.Framework;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file gntn_common_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoTrapScriptCallbackDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private UnityEngine.Object _scriptFile;

        [SerializeField, HideInInspector]
        private string scriptFilePath;

        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private bool _didAddParam;

        /// <inheritdoc />
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.scriptFilePath, out this._scriptFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("funcName", Core.PropertyInfoType.String, (this._funcName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("scriptFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._scriptFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("didAddParam", Core.PropertyInfoType.Bool, (this._didAddParam)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "funcName":
                    this._funcName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "scriptFile":
                    this.scriptFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "didAddParam":
                    this._didAddParam = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
            }
        }
    }
}
