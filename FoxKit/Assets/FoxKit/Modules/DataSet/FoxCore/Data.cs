namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;
    
    using Sirenix.Serialization;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine entities with explicit names.
    /// </summary>
    [Serializable]
    public class Data : Entity
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 72)]
        private string name;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.EntityHandle, 80, readable: PropertyExport.Never, writable: PropertyExport.Never)]
        private DataSet dataSet;
        
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var nameProperty = PropertyInfoFactory.MakeStaticArrayProperty("name", Core.PropertyInfoType.String, this.Name);
            var dataSetProperty = PropertyInfoFactory.MakeStaticArrayProperty("dataSet", Core.PropertyInfoType.EntityHandle, getEntityAddress(this.GetDataSet()));
            var properties = new List<Core.PropertyInfo> { nameProperty, dataSetProperty };
            return properties;
        }
        
        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "name")
            {
                this.name = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
            }
        }

        public DataSet GetDataSet()
        {
            return this.dataSet;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {this.Name}";
        }
    }
}