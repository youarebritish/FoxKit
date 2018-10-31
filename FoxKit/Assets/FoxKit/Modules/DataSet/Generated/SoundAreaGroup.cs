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
    public class SoundAreaGroup : Data
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private uint _priority;

        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private Entity _parameter;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaGroup")]
        private List<FoxCore.EntityLink> _members;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaGroup")]
        private List<FoxCore.EntityLink> _edges;

        /// <inheritdoc />
        public override short ClassId => 112;

        /// <inheritdoc />
        public override ushort Version => 3;
    }
}
