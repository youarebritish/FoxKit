﻿using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Modules.DataSet.Importer;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;

namespace FoxKit.Modules.DataSet.GameCore
{
    public class GameObject : Data
    {
        public string TypeName;
        public uint GroupId;
        public uint TotalCount;
        public uint RealizedCount;
        public GameObjectParameter Parameters;

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
            else if (propertyData.Name == "totalCount")
            {
                TotalCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
            }
            else if (propertyData.Name == "realizedCount")
            {
                RealizedCount = DataSetUtils.GetStaticArrayPropertyValue<FoxUInt32>(propertyData).Value;
            }
            else if (propertyData.Name == "parameters")
            {
                var address = DataSetUtils.GetStaticArrayPropertyValue<FoxEntityPtr>(propertyData).EntityPtr;
                Parameters = getEntity(address) as GameObjectParameter;

                if (Parameters != null)
                {
                    Parameters.Owner = this;
                }
            }
        }
    }
}