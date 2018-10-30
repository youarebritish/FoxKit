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
    public class SoundAreaMember : Data
    {
        [OdinSerialize, Modules.DataSet.Property("SoundAreaMember")]
        private List<FoxCore.EntityLink> _shapes;

        [SerializeField, Modules.DataSet.Property("SoundAreaMember")]
        private uint _priority;

        [OdinSerialize, Modules.DataSet.Property("SoundAreaMember")]
        private Entity _parameter;

        /// <inheritdoc />
        public override short ClassId => 96;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("shapes", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._shapes select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("priority", Core.PropertyInfoType.UInt32, (this._priority)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("parameter", Core.PropertyInfoType.EntityPtr, getEntityAddress(this._parameter)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "shapes":
                    this._shapes = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "priority":
                    this._priority = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "parameter":
                    this._parameter = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
            }
        }
    }
}
