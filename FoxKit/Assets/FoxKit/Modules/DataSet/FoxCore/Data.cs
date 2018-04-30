namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

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
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "name")
            {
                this.name = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}