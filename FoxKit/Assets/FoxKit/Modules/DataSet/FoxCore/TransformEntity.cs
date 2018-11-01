namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using FoxLib;

    using UnityEngine;

    using Quaternion = UnityEngine.Quaternion;
    using Vector3 = UnityEngine.Vector3;

    /// <inheritdoc />
    /// <summary>
    /// A DataElement attached to TransformData Entities representing a transform matrix.
    /// </summary>
    [Serializable]
    public class TransformEntity : DataElement
    {
        /// <summary>
        /// The scale.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 64)]
        private Vector3 transform_scale;

        /// <summary>
        /// The rotation.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Quat, 80)]
        private Quaternion transform_rotation_quat;

        /// <summary>
        /// The translation.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 96)]
        private Vector3 transform_translation;

        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Translation
        {
            get
            {
                return this.transform_translation;
            }
            set
            {
                this.transform_translation = value;
            }
        }

        public Quaternion RotQuat
        {
            get
            {
                return this.transform_rotation_quat;
            }
            set
            {
                this.transform_rotation_quat = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return this.transform_scale;
            }
            set
            {
                this.transform_scale = value;
            }
        }

        /// <inheritdoc />
        public override short ClassId => 80;
    }
}