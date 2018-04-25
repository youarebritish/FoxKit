using FoxKit.Modules.DataSet.FoxCore;
using System;
using UnityEngine;
using FoxKit.Modules.DataSet.Importer;
using FoxTool.Fox;
using FoxKit.Utils;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.GameCore
{
    [Serializable]
    public class GameObjectLocator : TransformData
    {
        public string TypeName;
        public uint GroupId;
        public GameObjectLocatorParameter Parameters;

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "typeName")
            {
                TypeName = DataSetUtils.GetStaticArrayPropertyValue<FoxString>(propertyData).ToString();
            }
            else if (propertyData.Name == "groupId")
            {
                GroupId = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
            }
            else if (propertyData.Name == "parameters")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                Parameters = getEntity(address) as GameObjectLocatorParameter;
                Parameters.Owner = this;
            }
        }
    }
}