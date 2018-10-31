namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TerrainDecalPack : Data
    {
        [SerializeField, Modules.DataSet.Property("TerrainDecalPack")]
        private UnityEngine.Object _terrainDecalPackFile;

        [SerializeField, HideInInspector]
        private string terrainDecalPackFilePath;

        [OdinSerialize, Modules.DataSet.Property("TerrainDecalPack")]
        private List<FoxCore.EntityLink> _materialLinks;

        /// <inheritdoc />
        public override short ClassId => 104;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
