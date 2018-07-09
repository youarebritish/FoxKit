namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;

    using NUnit.Framework;

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

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = new List<Core.PropertyInfo>();
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("array", Core.PropertyInfoType.EntityPtr, (from propertyEntry in this.array select getEntityAddress(propertyEntry) as object).ToArray()));
            return parentProperties;
        }


        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "array":
                    this.array = (from rawValue in DataSetUtils.GetDynamicArrayValues<ulong>(propertyData) select initFunctions.GetEntityFromAddress(rawValue)).ToList();
                    break;
            }
        }

    }
}
