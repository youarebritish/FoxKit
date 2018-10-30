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

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;

    // Automatically generated from file afgh_field_asset.fox2
    /// <inheritdoc />
    [Serializable]
    public class TppLadderData : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppLadderData")]
        private uint _numSteps;

        [SerializeField, Modules.DataSet.Property("TppLadderData")]
        private string _tacticalActionId = string.Empty;

        [OdinSerialize, Modules.DataSet.Property("TppLadderData")]
        private List<FoxCore.EntityLink> _entryPoints;

        /// <inheritdoc />
        public override short ClassId => 288;

        /// <inheritdoc />
        public override ushort Version => 3;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("numSteps", Core.PropertyInfoType.UInt32, (this._numSteps)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("tacticalActionId", Core.PropertyInfoType.String, (this._tacticalActionId)));
            parentProperties.Add(PropertyInfoFactory.MakeDynamicArrayProperty("entryPoints", Core.PropertyInfoType.EntityLink, (from propertyEntry in this._entryPoints select convertEntityLink(propertyEntry) as object).ToArray()));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "numSteps":
                    this._numSteps = (DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData));
                    break;
                case "tacticalActionId":
                    this._tacticalActionId = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "entryPoints":
                    this._entryPoints = (from rawValue in DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData) select initFunctions.MakeEntityLink(rawValue)).ToList();
                    break;
            }
        }
    }
}
