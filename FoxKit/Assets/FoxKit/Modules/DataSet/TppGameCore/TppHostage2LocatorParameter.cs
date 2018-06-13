namespace FoxKit.Modules.DataSet.TppGameCore
{
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;

    using FoxLib;

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
        protected override short ClassId => 32;

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