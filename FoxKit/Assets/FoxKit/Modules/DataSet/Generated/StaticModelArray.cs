namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.PartsBuilder;
    using FoxKit.Modules.DataSet.Sdx;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class StaticModelArray : Data
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 120)]
        private UnityEngine.Object modelFile;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 144)]
        private UnityEngine.Object geomFile;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 168)]
        private bool isVisibleGeom;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 176)]
        private float lodFarSize;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 172)]
        private float lodNearSize;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 180)]
        private float lodPolygonSize;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 184, enumType: typeof(DrawRejectionLevel))]
        private DrawRejectionLevel drawRejectionLevel;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 188, enumType: typeof(DrawMode))]
        private DrawMode drawMode;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 192, enumType: typeof(RejectFarRangeShadowCast))]
        private RejectFarRangeShadowCast rejectFarRangeShadowCast;

        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 232)]
        private FoxCore.EntityLink parentLocator = new EntityLink();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Matrix4, 200, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Matrix4x4> transforms;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 216, container: Core.ContainerType.DynamicArray)]
        private List<uint> colors;

        /// <inheritdoc />
        public override short ClassId => 208;

        /// <inheritdoc />
        public override ushort Version => 4;
    }
}
