namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;
    using FoxKit.Utils.UI.StringMap;

    using FoxLib;
    
    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class NavxAttributeInfo : DataElement
    {
        [SerializeField, Modules.DataSet.Property("NavxAttributeInfo")]
        private string _name = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxAttributeInfo")]
        private float _simplificationThreshold;

        /// <inheritdoc />
        public override short ClassId => 36;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("name", Core.PropertyInfoType.String, (this._name)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("simplificationThreshold", Core.PropertyInfoType.Float, (this._simplificationThreshold)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "name":
                    this._name = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "simplificationThreshold":
                    this._simplificationThreshold = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
            }
        }
    }
}
