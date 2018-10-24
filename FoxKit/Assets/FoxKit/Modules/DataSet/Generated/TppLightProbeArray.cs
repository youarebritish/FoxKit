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

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLightProbeArray : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private List<int> _drawRejectionLevels;

        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private List<FoxCore.EntityLink> _relatedLights;

        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private List<FoxCore.EntityLink> _shDatas;

        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private UnityEngine.Object _lightArrayFile;

        [SerializeField, HideInInspector]
        private string lightArrayFilePath;

        /// <inheritdoc />
        public override short ClassId => 336;

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
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("drawRejectionLevels", Core.PropertyInfoType.Int32, (from propertyEntry in this._drawRejectionLevels select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("relatedLights", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._relatedLights select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("shDatas", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._shDatas select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lightArrayFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._lightArrayFile)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "drawRejectionLevels":
                    this._drawRejectionLevels = (from rawValue in DataSetUtils.GetDynamicArrayValues<int>(propertyData) select (rawValue)).ToList();
                    break;
                case "relatedLights":
                    this._relatedLights = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "shDatas":
                    this._shDatas = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "lightArrayFile":
                    this.lightArrayFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
            }
        }
    }
}
