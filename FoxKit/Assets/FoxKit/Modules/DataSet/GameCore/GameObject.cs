namespace FoxKit.Modules.DataSet.GameCore
{
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// A dynamic Fox Engine Entity, which can be one of many types.
    /// </summary>
    public class GameObject : Data
    {
        /// <summary>
        /// Name of the GameObject type. This indicates the type of GameObject to spawn.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private string typeName = string.Empty;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint groupId;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint totalCount;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private uint realizedCount;

        /// <summary>
        /// Type-specific parameters.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("GameObject")]
        private GameObjectParameter parameters;

        /// <inheritdoc />
        protected override short ClassId => 88;

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
                case "totalCount":
                    this.totalCount = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "realizedCount":
                    this.realizedCount = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "parameters":
                    var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                    this.parameters = initFunctions.GetEntityFromAddress(address) as GameObjectParameter;
                    Assert.IsNotNull(this.parameters, $"Parameters for {this.name} was null.");

                    this.parameters.Owner = this;
                    break;
            }
        }
    }
}