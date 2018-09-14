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
    public class SoundAreaParameter : DataElement
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _ambientEvent = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _ambientRtpcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _ambientRtpcValue;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private string _objectRtpcName = string.Empty;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _objectRtpcValue;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private OrderedDictionary_string_float _auxSends;

        [SerializeField, Modules.DataSet.Property("SoundAreaParameter")]
        private float _dryVolume;

        /// <inheritdoc />
        public override short ClassId => 104;

        /// <inheritdoc />
        public override ushort Version => 4;

        /// <inheritdoc />
        public override List<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            var parentProperties = base.MakeWritableStaticProperties(getEntityAddress, convertEntityLink);
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ambientEvent", Core.PropertyInfoType.String, (this._ambientEvent)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ambientRtpcName", Core.PropertyInfoType.String, (this._ambientRtpcName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("ambientRtpcValue", Core.PropertyInfoType.Float, (this._ambientRtpcValue)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("objectRtpcName", Core.PropertyInfoType.String, (this._objectRtpcName)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("objectRtpcValue", Core.PropertyInfoType.Float, (this._objectRtpcValue)));
            parentProperties.Add(PropertyInfoFactory.MakeStringMapProperty("auxSends", Core.PropertyInfoType.Float, this._auxSends.ToDictionary(entry => entry.Key, entry => (entry.Value) as object)));
            parentProperties.Add(PropertyInfoFactory.MakeStaticArrayProperty("dryVolume", Core.PropertyInfoType.Float, (this._dryVolume)));
            return parentProperties;
        }

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "ambientEvent":
                    this._ambientEvent = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "ambientRtpcName":
                    this._ambientRtpcName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "ambientRtpcValue":
                    this._ambientRtpcValue = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "objectRtpcName":
                    this._objectRtpcName = (DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData));
                    break;
                case "objectRtpcValue":
                    this._objectRtpcValue = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
                case "auxSends":
                    var auxSendsDictionary = DataSetUtils.GetStringMap<float>(propertyData);
                    var auxSendsFinalValues = new OrderedDictionary_string_float();
                    
                    foreach(var entry in auxSendsDictionary)
                    {
                        auxSendsFinalValues.Add(entry.Key, (entry.Value));
                    }
                    
                    this._auxSends = auxSendsFinalValues;
                    break;
                case "dryVolume":
                    this._dryVolume = (DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData));
                    break;
            }
        }
    }
}
