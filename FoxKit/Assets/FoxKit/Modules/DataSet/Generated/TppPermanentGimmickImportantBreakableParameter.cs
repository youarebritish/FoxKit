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

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppPermanentGimmickImportantBreakableParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _life;

        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _flag1;

        [SerializeField, Modules.DataSet.Property("TppPermanentGimmickImportantBreakableParameter")]
        private uint _flag2;

        /// <inheritdoc />
        public override short ClassId => 40;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
