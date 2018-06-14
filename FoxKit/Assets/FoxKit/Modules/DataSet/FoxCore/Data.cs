namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine entities with explicit names.
    /// </summary>
    [Serializable]
    public abstract class Data : Entity
    {
        /// <summary>
        /// Just use the ScriptableObject's name for now.
        /// </summary>
        public string Name => this.name;
        
        /// <summary>
        /// Gets the DataSet that owns this Entity.
        /// </summary>
        /// <returns>
        /// The <see cref="DataSet"/> that owns this Entity.
        /// </returns>
        public DataSet GetDataSet()
        {
            return this.DataSet;
        }

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var nameProperty = PropertyInfoFactory.MakeStaticArrayProperty("name", Core.PropertyInfoType.String, this.Name);
            var dataSetProperty = PropertyInfoFactory.MakeStaticArrayProperty("dataSet", Core.PropertyInfoType.EntityHandle, getEntityAddress(this.DataSet));
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
    }
}