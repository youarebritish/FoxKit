namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppRainFilterInterruptTrans : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<UnityEngine.Matrix4x4> _planeMatrices;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<string> _maskTextures;

        [SerializeField, HideInInspector]
        private List<string> maskTexturesPath;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<uint> _interruptFlags;

        [SerializeField, Modules.DataSet.Property("TppRainFilterInterruptTrans")]
        private List<uint> _levels;

        /// <inheritdoc />
        public override short ClassId => 400;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
