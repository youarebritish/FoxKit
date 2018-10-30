namespace FoxKit.Modules.DataSet.PartsBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Base class for a PartsFile Data Entity.
    /// </summary>
    [Serializable]
    public abstract class PartDescription : Data
    {
        /// <summary>
        /// Other PartDescription Entities that this depends on.
        /// </summary>
        [OdinSerialize, Modules.DataSet.Property("Part Description")]
        private List<EntityLink> depends = new List<EntityLink>();

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Part Description")]
        private string partName = string.Empty;

        /// <summary>
        /// Not sure what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Part Description")]
        private string buildType = string.Empty;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(MeshFilter)).image as Texture2D;
    }
}