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
    public class BoxShape : TransformData
    {
        /// <inheritdoc />
        public override short ClassId => 256;

        /// <inheritdoc />
        public override ushort Version => 0;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
            }
        }

    }
}
