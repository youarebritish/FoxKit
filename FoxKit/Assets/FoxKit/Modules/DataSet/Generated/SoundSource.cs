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

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class SoundSource : TransformData
    {
        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private string _eventName = string.Empty;

        [OdinSerialize, Modules.DataSet.Property("SoundSource")]
        private List<FoxCore.EntityLink> _shapes;

        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private float _lodRange;

        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private float _playRange;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
