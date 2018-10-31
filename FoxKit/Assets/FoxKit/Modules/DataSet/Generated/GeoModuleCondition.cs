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
    public class GeoModuleCondition : TransformData
    {
        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private string _trapCategory = string.Empty;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private uint _trapPriority;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _enable;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _isOnce;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private bool _isAndCheck;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<string> _checkFuncNames;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<string> _execFuncNames;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<Entity> _checkCallbackDataElements;

        [SerializeField, Modules.DataSet.Property("GeoModuleCondition")]
        private List<Entity> _execCallbackDataElements;

        /// <inheritdoc />
        public override short ClassId => 352;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
