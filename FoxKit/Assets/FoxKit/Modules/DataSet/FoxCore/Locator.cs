namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// An Entity with a physical location in the world, usually used to group and position other TransformData Entities.
    /// </summary>
    [Serializable]
    public class Locator : TransformData
    {
        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField]
        private float size = 1.0f;

        /// <inheritdoc />
        protected override short ClassId => 272;

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "size")
            {
                this.size = DataSetUtils.GetStaticArrayPropertyValue<FoxFloat>(propertyData).Value;
            }
        }
    }
}