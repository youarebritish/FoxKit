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
    public class NavxAttributePathVolume : TransformData
    {
        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _nodes;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _edges;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private UnityEngine.Vector3 _topPos;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private string _worldName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxAttributePathVolume")]
        private List<Entity> _attributeInfos;

        /// <inheritdoc />
        public override short ClassId => 336;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
