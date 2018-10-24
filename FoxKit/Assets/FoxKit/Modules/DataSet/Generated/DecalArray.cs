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
    public class DecalArray : TransformData
    {
        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private FoxCore.EntityLink _material;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private int _projectionMode;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private float _nearClipScale;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private int _projectionTarget;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private float _repeatU;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private float _repeatV;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private float _transparency;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private int _polygonDataSource;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private int _drawRejectionLevel;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private float _drawRejectionDegree;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private uint _decalFlags;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<UnityEngine.Vector3> _scales;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<UnityEngine.Quaternion> _rotations;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<UnityEngine.Vector3> _translations;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<FoxCore.EntityLink> _targets;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<uint> _targetIndices;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<uint> _targetStartIndices;

        [SerializeField, Modules.DataSet.Property("DecalArray")]
        private List<int> _renderingPriorities;

        /// <inheritdoc />
        public override short ClassId => 448;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("material", Core.PropertyInfoType.EntityLink, convertEntityLink(this._material)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("projectionMode", Core.PropertyInfoType.Int32, (this._projectionMode)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("nearClipScale", Core.PropertyInfoType.Float, (this._nearClipScale)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("projectionTarget", Core.PropertyInfoType.Int32, (this._projectionTarget)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("repeatU", Core.PropertyInfoType.Float, (this._repeatU)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("repeatV", Core.PropertyInfoType.Float, (this._repeatV)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("transparency", Core.PropertyInfoType.Float, (this._transparency)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("polygonDataSource", Core.PropertyInfoType.Int32, (this._polygonDataSource)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawRejectionLevel", Core.PropertyInfoType.Int32, (this._drawRejectionLevel)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("drawRejectionDegree", Core.PropertyInfoType.Float, (this._drawRejectionDegree)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("decalFlags", Core.PropertyInfoType.UInt32, (this._decalFlags)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("scales", Core.PropertyInfoType.Vector3, (from propertyEntry in this._scales select FoxUtils.UnityToFox(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("rotations", Core.PropertyInfoType.Quat, (from propertyEntry in this._rotations select FoxUtils.UnityToFox(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("translations", Core.PropertyInfoType.Vector3, (from propertyEntry in this._translations select FoxUtils.UnityToFox(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("targets", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._targets select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("targetIndices", Core.PropertyInfoType.UInt32, (from propertyEntry in this._targetIndices select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("targetStartIndices", Core.PropertyInfoType.UInt32, (from propertyEntry in this._targetStartIndices select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("renderingPriorities", Core.PropertyInfoType.Int32, (from propertyEntry in this._renderingPriorities select (propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "material":
                    this._material = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "projectionMode":
                    this._projectionMode = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "nearClipScale":
                    this._nearClipScale = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "projectionTarget":
                    this._projectionTarget = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "repeatU":
                    this._repeatU = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "repeatV":
                    this._repeatV = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "transparency":
                    this._transparency = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "polygonDataSource":
                    this._polygonDataSource = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "drawRejectionLevel":
                    this._drawRejectionLevel = (DataSetUtils.GetStaticArrayPropertyValue<int>(propertyData));
                    break;
                case "drawRejectionDegree":
                    this._drawRejectionDegree = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "decalFlags":
                    this._decalFlags = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "scales":
                    this._scales = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.Vector3>(propertyData) select FoxUtils.FoxToUnity(rawValue)).ToList();
                    break;
                case "rotations":
                    this._rotations = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.Quaternion>(propertyData) select FoxUtils.FoxToUnity(rawValue)).ToList();
                    break;
                case "translations":
                    this._translations = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.Vector3>(propertyData) select FoxUtils.FoxToUnity(rawValue)).ToList();
                    break;
                case "targets":
                    this._targets = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "targetIndices":
                    this._targetIndices = (from rawValue in DataSetUtils.GetDynamicArrayValues<uint>(propertyData) select (rawValue)).ToList();
                    break;
                case "targetStartIndices":
                    this._targetStartIndices = (from rawValue in DataSetUtils.GetDynamicArrayValues<uint>(propertyData) select (rawValue)).ToList();
                    break;
                case "renderingPriorities":
                    this._renderingPriorities = (from rawValue in DataSetUtils.GetDynamicArrayValues<int>(propertyData) select (rawValue)).ToList();
                    break;
            }
        }
    }
}
