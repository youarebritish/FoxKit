namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLadderData : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppLadderData")]
        private uint _numSteps;

        [SerializeField, Modules.DataSet.Property("TppLadderData")]
        private string _tacticalActionId = string.Empty;

        [OdinSerialize, Modules.DataSet.Property("TppLadderData")]
        private List<FoxCore.EntityLink> _entryPoints;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 3;
    }
}
