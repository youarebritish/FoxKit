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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class LightArray : TransformData
    {
        [SerializeField, Modules.DataSet.Property("LightArray")]
        private UnityEngine.Object _lightArrayFile;

        [SerializeField, HideInInspector]
        private string lightArrayFilePath;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.lightArrayFilePath, out this._lightArrayFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lightArrayFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._lightArrayFile)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "lightArrayFile":
                    this.lightArrayFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}