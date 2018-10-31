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
    public class TppRequestWeatherTagTrapExecDataElement : DataElement
    {
        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private string _funcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private string _tagName = string.Empty;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private byte _priority;

        [SerializeField, Modules.DataSet.Property("TppRequestWeatherTagTrapExecDataElement")]
        private float _interpTime;

        /// <inheritdoc />
        public override short ClassId => 44;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
