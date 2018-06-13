namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Utils;

    using FoxLib;

    using UnityEngine;

    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

    /// <inheritdoc />
    /// <summary>
    /// A DataElement attached to TransformData Entities representing a transform matrix.
    /// </summary>
    [Serializable]
    public class TransformEntity : DataElement<TransformData>
    {
        /// <summary>
        /// The translation.
        /// </summary>
        [SerializeField, Modules.DataSet.Property]
        private Vector3 translation;

        /// <summary>
        /// The rotation.
        /// </summary>
        [SerializeField, Modules.DataSet.Property]
        private Quaternion rotation;

        /// <summary>
        /// The scale.
        /// </summary>
        [SerializeField, Modules.DataSet.Property]
        private Vector3 scale;

        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Translation => this.translation;

        /// <inheritdoc />
        protected override short ClassId => 80;

        /// <inheritdoc />
        protected override void ReadProperty(Core.PropertyInfo propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "transform_translation":
                    var foxTranslation = DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData);
                    this.translation = DataSetUtils.FoxToolToUnity(foxTranslation);
                    break;
                case "transform_rotation_quat":
                    var foxRotation = DataSetUtils.GetStaticArrayPropertyValue<Core.Quaternion>(propertyData);
                    this.rotation = DataSetUtils.FoxToolToUnity(foxRotation);
                    break;
                case "transform_scale":
                    var foxScale = DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(propertyData);
                    this.scale = DataSetUtils.FoxToolToUnity(foxScale);
                    break;
            }
        }
    }
}