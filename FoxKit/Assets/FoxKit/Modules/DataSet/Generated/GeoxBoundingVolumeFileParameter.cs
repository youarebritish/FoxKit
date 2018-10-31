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

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoxBoundingVolumeFileParameter : Data
    {
        [SerializeField, Modules.DataSet.Property("GeoxBoundingVolumeFileParameter")]
        private UnityEngine.Vector3 _gridSize;

        /// <inheritdoc />
        public override short ClassId => 80;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
