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

    // Automatically generated from file afgh_commFacility_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class SoundAreaEdge : Data
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private Entity _parameter;

        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private FoxCore.EntityLink _prevArea;

        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private FoxCore.EntityLink _nextArea;

        /// <inheritdoc />
        public override short ClassId => 136;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("parameter", Core.PropertyInfoType.EntityPtr, getEntityAddress(this._parameter)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("prevArea", Core.PropertyInfoType.EntityLink, convertEntityLink(this._prevArea)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("nextArea", Core.PropertyInfoType.EntityLink, convertEntityLink(this._nextArea)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "parameter":
                    this._parameter = initFunctions.GetEntityFromAddress(DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData));
                    break;
                case "prevArea":
                    this._prevArea = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "nextArea":
                    this._nextArea = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
            }
        }
    }
}
