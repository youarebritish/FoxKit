using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Modules.DataSet.Importer;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;
using UnityEngine;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class TexturePackLoadConditioner : Data
    {
        public string TexturePackPath;

        protected override void ReadProperty(FoxProperty propertyData, EntityFactory.GetEntityFromAddressDelegate getEntity)
        {
            base.ReadProperty(propertyData, getEntity);

            if (propertyData.Name == "texturePackPath")
            {
                TexturePackPath = DataSetUtils.GetStaticArrayPropertyValue<FoxPath>(propertyData).ToString();
            }
        }
    }
}