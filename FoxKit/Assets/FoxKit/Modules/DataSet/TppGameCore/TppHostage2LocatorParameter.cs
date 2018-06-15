namespace FoxKit.Modules.DataSet.TppGameCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="GameObjectLocator"/> with the type TppHostage2.
    /// </summary>
    public class TppHostage2LocatorParameter : GameObjectLocatorParameter
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        [SerializeField]
        private string identifier;

        /// <inheritdoc />
        public override short ClassId => 32;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("identifier", Core.PropertyInfoType.String, this.identifier));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "identifier")
            {
                this.identifier = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
            }
        }
    }
}