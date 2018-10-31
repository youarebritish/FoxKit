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
    public class SoundAreaParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _ambientEvent = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _ambientRtpcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _ambientRtpcValue;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _objectRtpcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _objectRtpcValue;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private OrderedDictionary_string_float _auxSends;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _dryVolume;

        /// <inheritdoc />
        public override short ClassId => 104;

        /// <inheritdoc />
        public override ushort Version => 4;
    }
}
