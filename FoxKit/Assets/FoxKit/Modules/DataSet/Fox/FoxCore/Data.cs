namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Utils;

    using FoxLib;
    
    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Analytics;
    using UnityEngine.Assertions;

    public partial class Data
    {
        public override Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(Animator)).image as Texture2D;

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
                this.referencePath = value;
            }
        }

        public DataSet DataSet
        {
            get
            {
                return this.dataSet as DataSet;
            }
            set
            {
                this.dataSet = value;
                this.DataSetGuid = value.DataSetGuid;
            }
        }

        protected override void OnPropertiesLoaded()
        {
            base.OnPropertiesLoaded();

            this.referencePath = this.name;
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
        /// Get all sub-Entities owned by this Data Entity.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetSubEntities()
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