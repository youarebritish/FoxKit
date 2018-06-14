namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Utils;

    using FoxLib;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    /// <summary>
    /// An Entity with a physical location in the world, usually used to group and position other TransformData Entities.
    /// </summary>
    [Serializable]
    public class Locator : TransformData
    {
        /// <summary>
        /// TODO: Figure this out.
        /// </summary>
        [SerializeField, Modules.DataSet.Property("Locator")]
        private float size = 1.0f;

        /// <inheritdoc />
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Transform)).image as Texture2D;

        /// <inheritdoc />
        public override short ClassId => 272;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("size", Core.PropertyInfoType.Float, this.size));

            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            if (propertyData.Name == "size")
            {
                this.size = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
            }
        }
    }
}