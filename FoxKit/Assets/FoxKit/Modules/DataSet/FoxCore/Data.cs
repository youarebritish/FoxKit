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
        public virtual string Name => this.name;
        
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
    }
}