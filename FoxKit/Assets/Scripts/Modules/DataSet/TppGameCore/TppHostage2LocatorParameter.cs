using FoxKit.Modules.DataSet.GameCore;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    public class TppHostage2LocatorParameter : GameObjectLocatorParameter
    {
        public string Identifier;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "identifier")
            {
                Identifier = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}