namespace FoxKit.Modules.DataSet.Sdx
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

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
        [SerializeField, Modules.DataSet.Property("Path")]
        private string texturePackPath = string.Empty;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Texture)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 72;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("texturePackPath", Core.PropertyInfoType.Path, this.texturePackPath));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "texturePackPath")
            {
                this.texturePackPath = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
            }
        }
    }
}