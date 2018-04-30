namespace FoxKit.Modules.DataSet.TppGameCore
{
    using FoxKit.Modules.DataSet.GameCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    /// <inheritdoc />
    /// <summary>
    /// Parameters for a <see cref="GameObjectLocator"/> with the type TppHostage2.
    /// </summary>
    public class TppHostage2LocatorParameter : GameObjectLocatorParameter
    {
        /// <summary>
        /// TODO: Figure out.
        /// </summary>
        private string identifier;

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "identifier")
            {
                this.identifier = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}