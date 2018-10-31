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

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class NavxHoleSimplificationParameterVolume : TransformData
    {
        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private string _sceneName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private string _worldName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _convexThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _obbExpandThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _obbToAabbThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private float _smoothingThreshold;

        [SerializeField, Modules.DataSet.Property("NavxHoleSimplificationParameterVolume")]
        private bool _isNotClosePassage;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}
