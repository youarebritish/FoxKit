using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;
using UnityEngine;
using static FoxKit.Modules.DataSet.Importer.EntityFactory;

namespace FoxKit.Modules.DataSet.GameCore
{
    /// <summary>
    /// Base class for Entities with a physical location in the world.
    /// </summary>
    [Serializable]
    public class GameObjectLocator : TransformData
    {
        public string TypeName;
        public uint GroupId;        
        public GameObjectLocatorParameter Parameters;

        protected override void ReadProperty(FoxProperty propertyData, GetEntityFromAddressDelegate getEntity)
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
                //var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                //Parameters = getEntity(address) as GameObjectLocatorParameter;
            }
        }
    }
}