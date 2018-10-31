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

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class SoundAreaEdge : Data
    {
        [OdinSerialize, Modules.DataSet.Property("SoundAreaEdge")]
        private Entity _parameter;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaEdge")]
        private FoxCore.EntityLink _prevArea;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaEdge")]
        private FoxCore.EntityLink _nextArea;

        /// <inheritdoc />
        public override short ClassId => 136;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}
