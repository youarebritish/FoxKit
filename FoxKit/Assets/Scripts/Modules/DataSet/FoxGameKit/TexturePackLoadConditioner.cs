using FoxKit.Modules.DataSet.FoxCore;
using FoxKit.Utils;
using FoxTool.Fox;
using FoxTool.Fox.Types.Values;
using System;

namespace FoxKit.Modules.DataSet.Sdx
{
    [Serializable]
    public class TexturePackLoadConditioner : Data
    {
        public string TexturePackPath;

        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "texturePackPath")
            {
                TexturePackPath = DataSetUtils.GetStaticArrayPropertyValue<FoxPath>(propertyData).ToString();
            }
        }
    }
}