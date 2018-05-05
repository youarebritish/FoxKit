namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;

    using FoxKit.Utils;

    using FoxTool.Fox;
    using FoxTool.Fox.Types.Structs;

    using UnityEngine;

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
        [SerializeField]
        private Vector3 translation;

        /// <summary>
        /// The rotation.
        /// </summary>
        [SerializeField]
        private Quaternion rotation;

        /// <summary>
        /// The scale.
        /// </summary>
        [SerializeField]
        private Vector3 scale;

        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Translation => this.translation;

        /// <inheritdoc />
        protected override short ClassId => 80;

        /// <inheritdoc />
        protected override void ReadProperty(FoxProperty propertyData, Importer.EntityFactory.EntityInitializeFunctions initFunctions)
        {
            base.ReadProperty(propertyData, initFunctions);

            switch (propertyData.Name)
            {
                case "transform_translation":
                    var foxTranslation = DataSetUtils.GetStaticArrayPropertyValue<FoxVector3>(propertyData);
                    this.translation = DataSetUtils.FoxToolToUnity(foxTranslation);
                    break;
                case "transform_rotation_quat":
                    var foxRotation = DataSetUtils.GetStaticArrayPropertyValue<FoxQuat>(propertyData);
                    this.rotation = DataSetUtils.FoxToolToUnity(foxRotation);
                    break;
                case "transform_scale":
                    var foxScale = DataSetUtils.GetStaticArrayPropertyValue<FoxVector3>(propertyData);
                    this.scale = DataSetUtils.FoxToolToUnity(foxScale);
                    break;
            }
        }
    }
}