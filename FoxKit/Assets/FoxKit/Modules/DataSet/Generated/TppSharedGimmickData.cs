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

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("modelFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._modelFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("geomFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._geomFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("breakedModelFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._breakedModelFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("breakedGeomFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._breakedGeomFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("partsFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._partsFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("numDynamicGimmick", Core.PropertyInfoType.UInt32, (this._numDynamicGimmick)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("locaterFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._locaterFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("flags1", Core.PropertyInfoType.UInt32, (this._flags1)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("flags2", Core.PropertyInfoType.UInt32, (this._flags2)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "modelFile":
                    this.modelFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "geomFile":
                    this.geomFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "breakedModelFile":
                    this.breakedModelFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "breakedGeomFile":
                    this.breakedGeomFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "partsFile":
                    this.partsFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "numDynamicGimmick":
                    this._numDynamicGimmick = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "locaterFile":
                    this.locaterFilePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "flags1":
                    this._flags1 = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "flags2":
                    this._flags2 = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
            }
        }
    }
}
