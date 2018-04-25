using FoxKit.Modules.DataSet.GameCore;
using System.Collections.Generic;
using FoxKit.Modules.DataSet.Importer;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.TppGameCore
{
    public class TppHostage2LocatorParameter : GameObjectLocatorParameter
    {
        public string Identifier;

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "identifier")
            {
                Identifier = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}