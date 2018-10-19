namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.Exporter;
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
    public class TransformEntity : DataElement//<TransformData>
    {
        /// <summary>
        /// The translation.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 0)]
        private Vector3 transform_translation;

        /// <summary>
        /// The rotation.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Quat, 0)]
        private Quaternion transform_rotation_quat;

        /// <summary>
        /// The scale.
        /// </summary>
        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 0)]
        private Vector3 transform_scale;

        /// <summary>
        /// The translation.
        /// </summary>
        public Vector3 Translation => this.transform_translation;

        /// <inheritdoc />
        public override short ClassId => 80;
    }
}