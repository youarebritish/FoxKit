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
    public class StaticModelArrayLinkTarget : Data
    {
        [SerializeField, Modules.DataSet.Property("StaticModelArrayLinkTarget")]
        private Entity _staticModelArray;

        [SerializeField, Modules.DataSet.Property("StaticModelArrayLinkTarget")]
        private uint _arrayIndex;

        /// <inheritdoc />
        public override short ClassId => 72;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
