namespace FoxKit.Modules.DataSet
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class Material : Data
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 120)]
        private string materialName = string.Empty;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 128)]
        private UnityEngine.Object shader;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 136)]
        private string diffuseTexture;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 152)]
        private string srmTexture;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 144)]
        private string normalTexture;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 160)]
        private string materialMapTexture;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt8, 168)]
        private byte materialIndex;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Color, 176)]
        private UnityEngine.Color diffuseColor;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Color, 192)]
        private UnityEngine.Color specularColor;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Path, 208)]
        private UnityEngine.Object fmtrPath;
        
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 216)]
        private bool residentFlag;

        /// <inheritdoc />
        public override short ClassId => 176;

        /// <inheritdoc />
        public override ushort Version => 6;
    }
}
