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

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class StaticModelArray : Data
    {
        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private UnityEngine.Object _modelFile;

        [SerializeField, HideInInspector]
        private string modelFilePath;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private UnityEngine.Object _geomFile;

        [SerializeField, HideInInspector]
        private string geomFilePath;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private bool _isVisibleGeom;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private float _lodFarSize;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private float _lodNearSize;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private float _lodPolygonSize;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private int _drawRejectionLevel;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private int _drawMode;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private int _rejectFarRangeShadowCast;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private FoxCore.EntityLink _parentLocator;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private List<UnityEngine.Matrix4x4> _transforms;

        [SerializeField, Modules.DataSet.Property("StaticModelArray")]
        private List<uint> _colors;

        /// <inheritdoc />
        public override short ClassId => 208;

        /// <inheritdoc />
        public override ushort Version => 4;

        /// <inheritdoc />
        public override void OnAssetsImported(FoxKit.Core.AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            base.OnAssetsImported(tryGetAsset);

            tryGetAsset(this.modelFilePath, out this._modelFile);
            tryGetAsset(this.geomFilePath, out this._geomFile);
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("modelFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._modelFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("geomFile", Core.PropertyInfoType.FilePtr, DataSetUtils.AssetToFoxPath(this._geomFile)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("isVisibleGeom", Core.PropertyInfoType.Bool, (this._isVisibleGeom)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodFarSize", Core.PropertyInfoType.Float, (this._lodFarSize)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodNearSize", Core.PropertyInfoType.Float, (this._lodNearSize)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodPolygonSize", Core.PropertyInfoType.Float, (this._lodPolygonSize)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawRejectionLevel", Core.PropertyInfoType.Int32, (this._drawRejectionLevel)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawMode", Core.PropertyInfoType.Int32, (this._drawMode)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("rejectFarRangeShadowCast", Core.PropertyInfoType.Int32, (this._rejectFarRangeShadowCast)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("parentLocator", Core.PropertyInfoType.EntityLink, convertEntityLink(this._parentLocator)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("transforms", Core.PropertyInfoType.Matrix4, (from propertyEntry in this._transforms select FoxUtils.UnityToFox(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("colors", Core.PropertyInfoType.UInt32, (from propertyEntry in this._colors select (propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "modelFile":
                    this.modelFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "geomFile":
                    this.geomFilePath = DataSetUtils.ExtractFilePath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "isVisibleGeom":
                    this._isVisibleGeom = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
                case "lodFarSize":
                    this._lodFarSize = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "lodNearSize":
                    this._lodNearSize = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "lodPolygonSize":
                    this._lodPolygonSize = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "drawRejectionLevel":
                    this._drawRejectionLevel = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "drawMode":
                    this._drawMode = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "rejectFarRangeShadowCast":
                    this._rejectFarRangeShadowCast = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "parentLocator":
                    this._parentLocator = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "transforms":
                    this._transforms = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.Matrix4>(propertyData) select FoxUtils.FoxToUnity(rawValue)).ToList();
                    break;
                case "colors":
                    this._colors = (from rawValue in DataSetUtils.GetDynamicArrayValues<uint>(propertyData) select (rawValue)).ToList();
                    break;
            }
        }
    }
}
