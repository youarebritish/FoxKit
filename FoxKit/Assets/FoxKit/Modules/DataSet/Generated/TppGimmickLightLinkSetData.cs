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

    // Automatically generated from file afgh_bridge_light.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppGimmickLightLinkSetData : Data
    {
        [SerializeField, Modules.DataSet.Property("TppGimmickLightLinkSetData")]
        private uint _numLightGimmick;

        [SerializeField, Modules.DataSet.Property("TppGimmickLightLinkSetData")]
        private FoxCore.EntityLink _ownerGimmick;

        [SerializeField, Modules.DataSet.Property("TppGimmickLightLinkSetData")]
        private List<FoxCore.EntityLink> _lightList;

        /// <inheritdoc />
        public override short ClassId => 136;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("numLightGimmick", Core.PropertyInfoType.UInt32, (this._numLightGimmick)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ownerGimmick", Core.PropertyInfoType.EntityLink, convertEntityLink(this._ownerGimmick)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("lightList", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._lightList select convertEntityLink(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "numLightGimmick":
                    this._numLightGimmick = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "ownerGimmick":
                    this._ownerGimmick = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData));
                    break;
                case "lightList":
                    this._lightList = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
            }
        }
    }
}
