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
    public class Material : Data
    {
        [SerializeField, Modules.DataSet.Property("Material")]
        private string _materialName = string.Empty;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _shader = string.Empty;

        [SerializeField, HideInInspector]
        private string shaderPath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _diffuseTexture = string.Empty;

        [SerializeField, HideInInspector]
        private string diffuseTexturePath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _srmTexture = string.Empty;

        [SerializeField, HideInInspector]
        private string srmTexturePath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _normalTexture = string.Empty;

        [SerializeField, HideInInspector]
        private string normalTexturePath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _materialMapTexture = string.Empty;

        [SerializeField, HideInInspector]
        private string materialMapTexturePath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private byte _materialIndex;

        [SerializeField, Modules.DataSet.Property("Material")]
        private UnityEngine.Color _diffuseColor;

        [SerializeField, Modules.DataSet.Property("Material")]
        private UnityEngine.Color _specularColor;

        [SerializeField, Modules.DataSet.Property("Material")]
        private string _fmtrPath = string.Empty;

        [SerializeField, HideInInspector]
        private string fmtrPathPath;

        [SerializeField, Modules.DataSet.Property("Material")]
        private bool _residentFlag;

        /// <inheritdoc />
        public override short ClassId => 176;

        /// <inheritdoc />
        public override ushort Version => 6;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("materialName", Core.PropertyInfoType.String, (this._materialName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("shader", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._shader)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("diffuseTexture", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._diffuseTexture)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("srmTexture", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._srmTexture)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("normalTexture", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._normalTexture)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("materialMapTexture", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._materialMapTexture)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("materialIndex", Core.PropertyInfoType.UInt8, (this._materialIndex)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("diffuseColor", Core.PropertyInfoType.Color, FoxUtils.UnityColorToFoxColorRGBA(this._diffuseColor)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("specularColor", Core.PropertyInfoType.Color, FoxUtils.UnityColorToFoxColorRGBA(this._specularColor)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("fmtrPath", Core.PropertyInfoType.Path, FoxUtils.FoxPathToUnityPath(this._fmtrPath)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("residentFlag", Core.PropertyInfoType.Bool, (this._residentFlag)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "materialName":
                    this._materialName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "shader":
                    this.shaderPath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "diffuseTexture":
                    this.diffuseTexturePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "srmTexture":
                    this.srmTexturePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "normalTexture":
                    this.normalTexturePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "materialMapTexture":
                    this.materialMapTexturePath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "materialIndex":
                    this._materialIndex = (DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData));
                    break;
                case "diffuseColor":
                    this._diffuseColor = FoxUtils.FoxColorRGBAToUnityColor(DataSetUtils.GetStaticArrayPropertyValue<Core.ColorRGBA>(propertyData));
                    break;
                case "specularColor":
                    this._specularColor = FoxUtils.FoxColorRGBAToUnityColor(DataSetUtils.GetStaticArrayPropertyValue<Core.ColorRGBA>(propertyData));
                    break;
                case "fmtrPath":
                    this.fmtrPathPath = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "residentFlag":
                    this._residentFlag = (DataSetUtils.GetStaticArrayPropertyValue<bool>(propertyData));
                    break;
            }
        }
    }
}
