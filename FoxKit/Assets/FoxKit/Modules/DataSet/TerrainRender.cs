namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <inheritdoc />
    [Serializable]
    public class TerrainRender : TransformData
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object filePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object loadFilePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object dummyFilePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.FilePtr, 304)]
        private UnityEngine.Object filePtr;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float meterPerOneRepeat;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float meterPerPixel;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 304)]
        private bool isWireFrame;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 304)]
        private bool lodFlag;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 304)]
        private bool isDebugMaterial;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityLink, 304, 16)]
        private List<EntityLink> materials = new List<EntityLink>(16);

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float lodParam;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityLink, 304, container: Core.ContainerType.DynamicArray)]
        private List<EntityLink> materialConfigs = new List<EntityLink>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object packedAlbedoTexturePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object packedNormalTexturePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object packedSrmTexturePath;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt64, 304)]
        private ulong packedMaterialIdentify;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 304)]
        private bool isFourceUsePackedMaterialTexture;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304)]
        private UnityEngine.Object baseColorTexture;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float materialLodScale;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float materialLodNearOffset;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float materialLodFarOffset;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Float, 304)]
        private float materialLodHeightOffset;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Int32, 304)]
        private int worldTextureMode;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304)]
        private uint worldTextureDividedNumX;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304)]
        private uint worldTextureDividedNumZ;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 304, container: Core.ContainerType.DynamicArray)]
        private List<UnityEngine.Object> worldTextureTilePathes = new List<Object>();
    }
}