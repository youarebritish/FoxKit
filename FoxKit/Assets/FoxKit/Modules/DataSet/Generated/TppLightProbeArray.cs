namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLightProbeArray : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private List<int> _drawRejectionLevels;

        [OdinSerialize, Modules.DataSet.Property("TppLightProbeArray")]
        private List<FoxCore.EntityLink> _relatedLights;

        [OdinSerialize, Modules.DataSet.Property("TppLightProbeArray")]
        private List<FoxCore.EntityLink> _shDatas;

        [SerializeField, Modules.DataSet.Property("TppLightProbeArray")]
        private UnityEngine.Object _lightArrayFile;

        [SerializeField, HideInInspector]
        private string lightArrayFilePath;

        /// <inheritdoc />
        public override short ClassId => 336;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
