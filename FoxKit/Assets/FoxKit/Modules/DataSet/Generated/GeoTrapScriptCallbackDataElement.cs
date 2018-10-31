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

    // Automatically generated from file gntn_common_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoTrapScriptCallbackDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private UnityEngine.Object _scriptFile;

        [SerializeField, HideInInspector]
        private string scriptFilePath;

        [SerializeField, Modules.DataSet.Property("GeoTrapScriptCallbackDataElement")]
        private bool _didAddParam;

        /// <inheritdoc />
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
