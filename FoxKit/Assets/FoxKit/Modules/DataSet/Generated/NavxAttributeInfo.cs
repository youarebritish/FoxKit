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
    
    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class NavxAttributeInfo : DataElement
    {
        [SerializeField, Modules.DataSet.Property("NavxAttributeInfo")]
        private string _name = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxAttributeInfo")]
        private float _simplificationThreshold;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
