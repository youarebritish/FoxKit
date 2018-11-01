namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Utils;

    using FoxLib;
    
    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine entities with explicit names.
    /// </summary>
    [Serializable]
    public class Data : Entity
    {
        [SerializeField, PropertyInfo(Core.PropertyInfoType.String, 72)]
        private string name;

        [SerializeField, NonSerialized, PropertyInfo(Core.PropertyInfoType.EntityHandle, 80, readable: PropertyExport.Never, writable: PropertyExport.Never)]
        private Entity dataSet;

        /// <summary>
        /// Since we can't store a reference to the owning DataSet (recursive reference), we store its GUID instead.
        /// </summary>
        public string DataSetGuid;
        
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public DataSet GetDataSet()
        {
            Assert.IsNotNull(this.DataSetGuid);

            var dataSetPath = AssetDatabase.GUIDToAssetPath(this.DataSetGuid);
            Assert.IsNotNull(dataSetPath);

            return AssetDatabase.LoadAssetAtPath<DataSetAsset>(dataSetPath).GetDataSet();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: {this.Name}";
        }

        /// <summary>
        /// Get all DataElements owned by this Data Entity.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetDataElements()
        {
            var entityPtrProperties = from type in ReflectionUtils.GetParentTypes(this.GetType(), true)
                                      from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                      let attribute = field.GetCustomAttribute<PropertyInfoAttribute>()
                                      where attribute != null && attribute.Type == Core.PropertyInfoType.EntityPtr
                                      select new { Attribute = attribute, Value = field.GetValue(this) };

            var result = new List<Entity>();
            foreach (var property in entityPtrProperties)
            {
                switch (property.Attribute.Container)
                {
                    case Core.ContainerType.StaticArray:
                        if (property.Attribute.ArraySize == 1)
                        {
                            result.Add((Entity)property.Value);
                            break;
                        }

                        result.AddRange((IEnumerable<Entity>)property.Value);
                        break;
                    case Core.ContainerType.DynamicArray:
                        result.AddRange((IEnumerable<Entity>)property.Value);
                        break;
                    case Core.ContainerType.StringMap:
                        result.AddRange(((IDictionary<string, Entity>)property.Value).Values);
                        break;
                    case Core.ContainerType.List:
                        result.AddRange((IEnumerable<Entity>)property.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }
    }
}