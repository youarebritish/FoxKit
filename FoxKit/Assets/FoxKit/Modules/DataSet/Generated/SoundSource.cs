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

    using NUnit.Framework;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_bridge_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class SoundSource : TransformData
    {
        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private string _eventName = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private List<FoxCore.EntityLink> _shapes;

        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private float _lodRange;

        [SerializeField, Modules.DataSet.Property("SoundSource")]
        private float _playRange;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 2;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("eventName", Core.PropertyInfoType.String, (this._eventName)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("shapes", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._shapes select convertEntityLink(propertyEntry) as object).ToArray()));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("lodRange", Core.PropertyInfoType.Float, (this._lodRange)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("playRange", Core.PropertyInfoType.Float, (this._playRange)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "eventName":
                    this._eventName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "shapes":
                    this._shapes = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
                case "lodRange":
                    this._lodRange = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "playRange":
                    this._playRange = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
            }
        }
    }
}
