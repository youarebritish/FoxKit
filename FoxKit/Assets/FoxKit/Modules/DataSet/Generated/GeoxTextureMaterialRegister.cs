namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file gntn_common_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoxTextureMaterialRegister : Data
    {
        [OdinSerialize, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private FoxCore.EntityLink _materialLink;

        [SerializeField, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private string _collisionMaterialName = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoxTextureMaterialRegister")]
        private string _collisionColorName = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 200;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
