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
    public class SoundAreaMember : Data
    {
        [OdinSerialize, Modules.DataSet.Property("SoundAreaMember")]
        private List<FoxCore.EntityLink> _shapes;

        [SerializeField, Modules.DataSet.Property("SoundAreaMember")]
        private uint _priority;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaMember")]
        private Entity _parameter;

        /// <inheritdoc />
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
