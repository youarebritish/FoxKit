namespace FoxKit.Modules.DataSet.Sdx
{
    using System;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Values;

    /// <inheritdoc />
    /// <summary>
    /// Don't know what this is, but it looks like every DataSetFile needs one.
    /// </summary>
    [Serializable]
    public class TexturePackLoadConditioner : Data
    {
        /// <summary>
        /// The texture pack path.
        /// </summary>
        private string texturePackPath;

        /// <inheritdoc />
        protected override short ClassId => 72;

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "texturePackPath")
            {
                this.texturePackPath = DataSetUtils.GetStaticArrayPropertyValue<FoxPath>(propertyData).ToString();
            }
        }
    }
}