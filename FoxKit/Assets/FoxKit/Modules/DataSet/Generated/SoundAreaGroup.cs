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
    public class SoundAreaGroup : Data
    {
        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private uint priority;

        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private Entity parameter;

        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private List<EntityLink> members;

        [SerializeField, Modules.DataSet.Property("SoundAreaGroup")]
        private List<EntityLink> edges;

        /// <inheritdoc />
        public override short ClassId => 112;

        /// <inheritdoc />
        public override ushort Version => 3;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "priority":
                    this.priority = DataSetUtils.GetStaticArrayPropertyValue<uint>(propertyData);
                    break;
                case "parameter":
                    var parameterAddress = DataSetUtils.GetStaticArrayPropertyValue<ulong>(propertyData);
                    this.parameter = initFunctions.GetEntityFromAddress(parameterAddress);
                    Assert.IsNotNull(this.parameter);
                    break;
                case "members":
                    var membersRawEntityLink = DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData);
                    this.members = (from link in membersRawEntityLink select DataSetUtils.MakeEntityLink(this.GetDataSet(), link)).ToList();
                    break;
                case "edges":
                    var edgesRawEntityLink = DataSetUtils.GetDynamicArrayValues<Core.EntityLink>(propertyData);
                    this.edges = (from link in edgesRawEntityLink select DataSetUtils.MakeEntityLink(this.GetDataSet(), link)).ToList();
                    break;
            }
        }

    }
}
