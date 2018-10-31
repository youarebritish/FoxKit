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

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppAreaEdgeParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private uint _fadeTime;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedClearObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedClearOcclusion;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedBlockedObstruction;

        [SerializeField, Modules.DataSet.Property("TppAreaEdgeParameter")]
        private float _connectedBlockedOcclusion;

        /// <inheritdoc />
        public override short ClassId => 48;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
