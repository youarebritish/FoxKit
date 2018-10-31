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
    public class GraphxSpatialGraphDataEdge : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataEdge")]
        private Entity _prevNode;

        [SerializeField, Modules.DataSet.Property("GraphxSpatialGraphDataEdge")]
        private Entity _nextNode;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
