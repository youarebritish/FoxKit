namespace FoxKit.Modules.DataSet.GameCore
{
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

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
        [SerializeField, Category("GameObject")]
        private string typeName = string.Empty;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Category("GameObject")]
        private uint groupId;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Category("GameObject")]
        private uint totalCount;

        /// <summary>
        /// No idea what this is.
        /// </summary>
        [SerializeField, Category("GameObject")]
        private uint realizedCount;

        /// <summary>
        /// Type-specific parameters.
        /// </summary>
        [SerializeField, Category("GameObject")]
        private GameObjectParameter parameters;

        /// <inheritdoc />
        protected override short ClassId => 88;

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "typeName":
                    this.typeName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
                    break;
                case "groupId":
                    this.groupId = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    break;
                case "totalCount":
                    this.totalCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    break;
                case "realizedCount":
                    this.realizedCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
                    break;
                case "parameters":
                    var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                    this.parameters = initFunctions.GetEntityFromAddress(address) as GameObjectParameter;
                    Assert.IsNotNull(this.parameters, $"Parameters for {name} was null.");

                    this.parameters.Owner = this;
                    break;
            }
        }
    }
}