namespace FoxKit.Modules.DataSet
{
    using System;
    using System.Collections.Generic;

    using FoxKit.Modules.DataSet.FoxCore;

    using FoxLib;

    using OdinSerializer;

    using UnityEngine;

    using Vector3 = UnityEngine.Vector3;

    /// <inheritdoc />
    [Serializable]
    public class GeoxCollisionPyraidFreeShape : TransformData
    {
        [OdinSerialize, PropertyInfo(Core.PropertyInfoType.Int32, 384)]
        private int collisionCategory;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 160, readable: PropertyExport.EditorOnly, writable: PropertyExport.Never)]
        private string collisionMaterial;

        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 164, container: Core.ContainerType.DynamicArray)]
        private List<string> collisionAttributeNames = new List<string>();

        [SerializeField, PropertyInfo(Core.PropertyInfoType.Vector3, 168, 5)]
        private List<Vector3> points = new List<Vector3>(5);
    }
}