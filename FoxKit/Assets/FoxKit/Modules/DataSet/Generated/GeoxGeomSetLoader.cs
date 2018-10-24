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
    public class GeoxGeomSetLoader : TransformData
    {
        [SerializeField, Modules.DataSet.Property("GeoxGeomSetLoader")]
        private UnityEngine.Object geomSetFile;
        
        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
