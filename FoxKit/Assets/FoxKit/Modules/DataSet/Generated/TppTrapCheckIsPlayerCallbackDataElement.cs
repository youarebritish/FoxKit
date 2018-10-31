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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppTrapCheckIsPlayerCallbackDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppTrapCheckIsPlayerCallbackDataElement")]
        private string _funcName = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 32;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
