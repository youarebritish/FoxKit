namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class SoundAreaGroup : Data
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private uint _priority;

        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private Entity _parameter;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaGroup")]
        private List<FoxCore.EntityLink> _members;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaGroup")]
        private List<FoxCore.EntityLink> _edges;

        /// <inheritdoc />
        public override short ClassId => 112;

        /// <inheritdoc />
        public override ushort Version => 3;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("priority", Core.PropertyInfoType.UInt32, (this._priority)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("parameter", Core.PropertyInfoType.EntityPtr, getEntityAddress(this._parameter)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("members", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._members select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("edges", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._edges select convertEntityLink(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "priority":
                    this._priority = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "parameter":
                    this._parameter = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
                case "members":
                    this._members = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "edges":
                    this._edges = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
            }
        }
    }
}
