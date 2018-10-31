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
    public class FoxTrapExecViewGroupControlCallbackDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("FoxTrapExecViewGroupControlCallbackDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("FoxTrapExecViewGroupControlCallbackDataElement")]
        private string _identify = string.Empty;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
