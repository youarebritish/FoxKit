namespace FoxKit.Modules.DataSet.GameCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// A dynamic Fox Engine Entity, which can be one of many types.
    /// </summary>
    public class GameObject : Data
    {
        /// <summary>
        /// Name of the GameObject type. This indicates the type of GameObject to spawn.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private string typeName = string.Empty;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint groupId;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint totalCount;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint realizedCount;

        /// <summary>
        /// Type-specific parameters.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private GameObjectParameter parameters;

        /// <inheritdoc />
        public override short ClassId => 88;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}