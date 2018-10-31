namespace FoxKit.Modules.DataSet.FoxCore
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

    /// <inheritdoc />
    [Serializable]
    public class EntityPtrArrayEntity : Entity
    {
        [SerializeField, Modules.DataSet.Property("EntityPtrArrayEntity")]
        private List<Entity> array;

        /// <inheritdoc />
        public override short ClassId => 40;

        /// <inheritdoc />
        public override ushort Version => 0;
    }
}
