namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppGimmickElectricCableLinkSetData : Data
    {
        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private FoxCore.EntityLink _electricCableData;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private FoxCore.EntityLink _poleData;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<string> _electricCable;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<string> _pole;

        [SerializeField, Modules.DataSet.Property("TppGimmickElectricCableLinkSetData")]
        private List<byte> _cnpIndex;

        /// <inheritdoc />
        public override short ClassId => 176;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
