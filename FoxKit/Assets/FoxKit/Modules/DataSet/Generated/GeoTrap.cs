namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class GeoTrap : TransformData
    {
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 304, container: Core.ContainerType.DynamicArray)]
        private List<FoxCore.EntityLink> conditionArray;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Bool, 320)]
        private bool enable;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 2;
    }
}
