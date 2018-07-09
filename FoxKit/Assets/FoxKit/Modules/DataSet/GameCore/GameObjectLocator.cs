namespace FoxKit.Modules.DataSet.GameCore
{
    using System;
    using System.Collections.Generic;

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
        [SerializeField, Modules.DataSet.Property("GameObjectLocator")]
        private string typeName;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObjectLocator")]
        private uint groupId;

        /// <summary>
        /// Type-specific parameters.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObjectLocator")]
        private GameObjectLocatorParameter parameters;

        /// <inheritdoc />
        public override short ClassId => 272;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("typeName", Core.PropertyInfoType.String, this.typeName));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("groupId", Core.PropertyInfoType.UInt32, this.groupId));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("parameters", Core.PropertyInfoType.EntityPtr, getEntityAddress(this.parameters)));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "typeName":
                    this.typeName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "groupId":
                    this.groupId = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "parameters":
                    var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                    this.parameters = initFunctions.GetEntityFromAddress(address) as GameObjectLocatorParameter;
                    Assert.IsNotNull(this.parameters, $"Parameters for {this.name} was null.");

                    //this.parameters.Owner = this;
                    break;
            }
        }
    }
}