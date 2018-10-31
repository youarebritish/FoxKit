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
    /// A <see cref="GameObject"/> with a position? Not really sure how these classes differ.
    /// </summary>
    [Serializable]
    public class GameObjectLocator : TransformData
    {
        /// <summary>
        /// Name of the GameObject type. This indicates the type of GameObject to spawn.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 304)]
        private string typeName;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 304)]
        private uint groupId;

        /// <summary>
        /// Type-specific parameters.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityPtr, 304)]
        private GameObjectLocatorParameter parameters;

        /// <inheritdoc />
        public override short ClassId => 272;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}