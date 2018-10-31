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
    public class ViewGroupControlCondition : Data
    {
        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private uint _flags;

        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private int _condition;

        [SerializeField, Modules.DataSet.Property("ViewGroupControlCondition")]
        private string _identify = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 76;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
