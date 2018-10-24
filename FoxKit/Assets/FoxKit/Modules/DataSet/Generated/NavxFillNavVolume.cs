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
    public class NavxFillNavVolume : TransformData
    {
        [SerializeField, Modules.DataSet.Property("NavxFillNavVolume")]
        private string _sceneName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxFillNavVolume")]
        private string _worldName = string.Empty;

        [SerializeField, Modules.DataSet.Property("NavxFillNavVolume")]
        private List<string> _attributes;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("sceneName", Core.PropertyInfoType.String, (this._sceneName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("worldName", Core.PropertyInfoType.String, (this._worldName)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("attributes", Core.PropertyInfoType.String, (from propertyEntry in this._attributes select (propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "sceneName":
                    this._sceneName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "worldName":
                    this._worldName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "attributes":
                    this._attributes = (from rawValue in DataSetUtils.GetDynamicArrayValues<string>(propertyData) select (rawValue)).ToList();
                    break;
            }
        }
    }
}
