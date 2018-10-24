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
    public class TppRainFilterInterruptTrans : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<UnityEngine.Matrix4x4> _planeMatrices;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<string> _maskTextures;

        [SerializeField, HideInInspector]
        private List<string> maskTexturesPath;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<uint> _interruptFlags;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<uint> _levels;

        /// <inheritdoc />
        public override short ClassId => 400;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("planeMatrices", Core.PropertyInfoType.Matrix4, (from propertyEntry in this._planeMatrices select FoxUtils.UnityToFox(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("maskTextures", Core.PropertyInfoType.Path, (from propertyEntry in this._maskTextures select FoxUtils.FoxPathToUnityPath(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("interruptFlags", Core.PropertyInfoType.UInt32, (from propertyEntry in this._interruptFlags select (propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("levels", Core.PropertyInfoType.UInt32, (from propertyEntry in this._levels select (propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "planeMatrices":
                    this._planeMatrices = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.Matrix4>(propertyData) select FoxUtils.FoxToUnity(rawValue)).ToList();
                    break;
                case "maskTextures":
                    this.maskTexturesPath = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select FoxUtils.FoxPathToUnityPath(rawValue)).ToList();
                    break;
                case "interruptFlags":
                    this._interruptFlags = (from rawValue in DataSetUtils.GetDynamicArrayValues<uint>(propertyData) select (rawValue)).ToList();
                    break;
                case "levels":
                    this._levels = (from rawValue in DataSetUtils.GetDynamicArrayValues<uint>(propertyData) select (rawValue)).ToList();
                    break;
            }
        }
    }
}
