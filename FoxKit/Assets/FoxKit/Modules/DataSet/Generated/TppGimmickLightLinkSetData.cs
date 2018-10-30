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
    public class TppGimmickLightLinkSetData : Data
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.UInt32, 120)]
        private uint numLightGimmick;

        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 128)]
        private FoxCore.EntityLink ownerGimmick;

        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.EntityLink, 168, container: Core.ContainerType.DynamicArray)]
        private List<FoxCore.EntityLink> lightList;

        /// <inheritdoc />
        public override short ClassId => 136;

        /// <inheritdoc />
        public override ushort Version => 1;
    }
}
