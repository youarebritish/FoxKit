namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;

    using NUnit.Framework;

    using UnityEditor;

    using UnityEngine;

    /// <inheritdoc />
    [Serializable]
    public class TppAmbientSoundSource : TransformData
    {
        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private string eventName;

        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private List<EntityLink> shapes;

        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private float lodRange;

        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private float playRange;

        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private string volumeRtpc;

        [SerializeField, Modules.DataSet.Property("TppAmbientSoundSource")]
        private byte ambientIndex;

        /// <inheritdoc />
        public override short ClassId => 304;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "eventName":
                    this.eventName = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "shapes":
                    var shapesRawEntityLink = DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData);
                    this.shapes = (from link in shapesRawEntityLink select DataSetUtils.MakeEntityLink(this.GetDataSet(), link)).ToList();
                    break;
                case "lodRange":
                    this.lodRange = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "playRange":
                    this.playRange = DataSetUtils.GetStaticArrayPropertyValue<float>(propertyData);
                    break;
                case "volumeRtpc":
                    this.volumeRtpc = DataSetUtils.GetStaticArrayPropertyValue<string>(propertyData);
                    break;
                case "ambientIndex":
                    this.ambientIndex = DataSetUtils.GetStaticArrayPropertyValue<byte>(propertyData);
                    break;
            }
        }

    }
}
