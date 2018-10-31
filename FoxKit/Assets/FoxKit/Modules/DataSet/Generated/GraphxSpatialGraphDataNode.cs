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
    public class GraphxSpatialGraphDataNode : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private UnityEngine.Vector3 _position;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private List<Entity> _inlinks;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataNode")]
        private List<Entity> _outlinks;

        /// <inheritdoc />
        public override short ClassId => 80;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
