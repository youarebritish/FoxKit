namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class DecalArray : TransformData
    {
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 304)]
        private FoxCore.EntityLink material;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304)]
        private int projectionMode;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float nearClipScale;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304)]
        private int projectionTarget;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float repeatU;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float repeatV;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float transparency;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304)]
        private int polygonDataSource;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304)]
        private int drawRejectionLevel;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float drawRejectionDegree;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304)]
        private uint decalFlags;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 304, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Vector3> scales;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Quat, 304, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Quaternion> rotations;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 304, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Vector3> translations;

        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 304, container: Core.ContainerType.DynamicArray)]
        private List<FoxCore.EntityLink> targets;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304, 304, container: Core.ContainerType.DynamicArray)]
        private List<uint> targetIndices;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304, 304, container: Core.ContainerType.DynamicArray)]
        private List<uint> targetStartIndices;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304, 304, container: Core.ContainerType.DynamicArray)]
        private List<int> renderingPriorities;

        /// <inheritdoc />
        public override short ClassId => 448;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}
