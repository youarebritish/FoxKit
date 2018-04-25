using System;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using FoxKit.Utils;
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

namespace FoxKit.Modules.DataSet.FoxCore
{
    [Serializable]
    public abstract class Data : Entity
    {
        /// <summary>
        /// Just use the ScriptableObject's name.
        /// </summary>
        public string Name => name;

        protected override void ReadProperty(FoxProperty propertyData)//, GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData);//, getEntity);

            if (propertyData.Name == "name")
            {
                name = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
        }
    }
}