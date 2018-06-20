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
    public class SoundAreaEdge : Data
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private Entity parameter;

        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private EntityLink prevArea;

        [SerializeField, Modules.DataSet.Property("SoundAreaEdge")]
        private EntityLink nextArea;

        /// <inheritdoc />
        public override short ClassId => 136;

        /// <inheritdoc />
        public override ushort Version => 1;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "parameter":
                    var parameterAddress = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                    this.parameter = initFunctions.GetEntityFromAddress(parameterAddress);
                    Assert.IsNotNull(this.parameter);
                    break;
                case "prevArea":
                    var prevAreaRawEntityLink = DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData);
                    this.prevArea = DataSetUtils.MakeEntityLink(this.GetDataSet(), prevAreaRawEntityLink);
                    break;
                case "nextArea":
                    var nextAreaRawEntityLink = DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(propertyData);
                    this.nextArea = DataSetUtils.MakeEntityLink(this.GetDataSet(), nextAreaRawEntityLink);
                    break;
            }
        }

    }
}
