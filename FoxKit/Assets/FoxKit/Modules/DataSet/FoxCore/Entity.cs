namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Importer;
    using FoxKit.Modules.Lua;
    using FoxKit.Utils;

    using FoxLib;

    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    using AssetPostprocessor = FoxKit.Core.AssetPostprocessor;
    using static KopiLua.Lua;

    using Object = UnityEngine.Object;

    /// <inheritdoc />
    /// <summary>
    /// Base class for Fox Engine objects.
    /// </summary>
    [Serializable]
    [ExposeClassToLua]
    public class Entity
    {
        /// <summary>
        /// Files that this Entity is looking for, along with the name of the field that wants the file.
        /// </summary>
        [OdinSerialize]
        private Dictionary<string, string> desiredFiles = new Dictionary<string, string>();

        /// <summary>
        /// The icon to use in the Data List window.
        /// </summary>
        public virtual Texture2D Icon => EditorGUIUtility.ObjectContent(null, typeof(GameObject)).image as Texture2D;

        /// <summary>
        /// ID of the class.
        /// </summary>
        public virtual short ClassId => -1;

        /// <summary>
        /// Version of the class.
        /// </summary>
        public virtual ushort Version => 2;
        
        /// <summary>
        /// Initializes the Entity with data loaded from a DataSet file.
        /// </summary>
        /// <param name="entityData">
        /// The data loaded from a DataSet file.
        /// </param>
        /// <param name="initFunctions">
        /// Helper functions to aid in initialization.
        /// </param>
        public void Initialize(Core.Entity entityData, EntityFactory.EntityInitializeFunctions initFunctions)
        {
            /*foreach (var property in entityData.StaticProperties)
            {
                this.ReadProperty(property, initFunctions);
            }*/

            this.ReadStaticProperties(entityData.StaticProperties, initFunctions);

            foreach (var unused in entityData.DynamicProperties)
            {
                Debug.LogError($"Attempted to read dynamic property in an entity of type {entityData.ClassName} but dynamic properties are not yet supported.");
            }
        }

        private void ReadStaticProperties(Core.PropertyInfo[] properties, EntityFactory.EntityInitializeFunctions initFunctions)
        {
            var baseTypes = ReflectionUtils.GetParentTypes(this.GetType());
            baseTypes.Add(this.GetType());

            var fields = (from type in baseTypes
                         from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                         let attribute = field.GetCustomAttribute<PropertyInfoAttribute>()
                         where attribute != null
                         select new { Field = field, PropertyInfo = attribute }).ToList();

            foreach (var field in fields)
            {
                var loadedProperty = properties.FirstOrDefault(property => property.Name == field.Field.Name);
                Assert.IsNotNull(loadedProperty);

                var propertyInfo = field.PropertyInfo;

                // Case 1: Non-array fields (the easiest)
                if (propertyInfo.Container == Core.ContainerType.StaticArray && propertyInfo.ArraySize == 1)
                {
                    object value = null;
                    switch (propertyInfo.Type)
                    {
                        case Core.PropertyInfoType.Int8:
                            value = DataSetUtils.GetStaticArrayPropertyValue<byte>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.UInt8:
                            value = DataSetUtils.GetStaticArrayPropertyValue<sbyte>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Int16:
                            value = DataSetUtils.GetStaticArrayPropertyValue<short>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.UInt16:
                            value = DataSetUtils.GetStaticArrayPropertyValue<ushort>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Int32:
                            value = DataSetUtils.GetStaticArrayPropertyValue<int>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.UInt32:
                            value = DataSetUtils.GetStaticArrayPropertyValue<uint>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Int64:
                            value = DataSetUtils.GetStaticArrayPropertyValue<long>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.UInt64:
                            value = DataSetUtils.GetStaticArrayPropertyValue<ulong>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Float:
                            value = DataSetUtils.GetStaticArrayPropertyValue<float>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Double:
                            value = DataSetUtils.GetStaticArrayPropertyValue<double>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Bool:
                            value = DataSetUtils.GetStaticArrayPropertyValue<bool>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.String:
                            value = DataSetUtils.GetStaticArrayPropertyValue<string>(loadedProperty);
                            break;
                        case Core.PropertyInfoType.Path:
                            value = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.EntityPtr:
                            var address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(loadedProperty);
                            value = initFunctions.GetEntityFromAddress(address);
                            break;
                        case Core.PropertyInfoType.Vector3:
                            value = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector3>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.Vector4:
                            value = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Vector4>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.Quat:
                            value = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Quaternion>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.Matrix3:
                            value = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Matrix3>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.Matrix4:
                            value = FoxUtils.FoxToUnity(DataSetUtils.GetStaticArrayPropertyValue<Core.Matrix4>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.Color:
                            value = FoxUtils.FoxColorRGBAToUnityColor(DataSetUtils.GetStaticArrayPropertyValue<Core.ColorRGBA>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.FilePtr:
                            // The file might not have been imported yet, so we need to hold onto the path and listen for it whenever assets are imported.
                            var path = FoxUtils.FoxPathToUnityPath(DataSetUtils.GetStaticArrayPropertyValue<string>(loadedProperty));
                            this.desiredFiles.Add(path, field.Field.Name);
                            break;
                        case Core.PropertyInfoType.EntityHandle:
                            address = DataSetUtils.GetStaticArrayPropertyValue<ulong>(loadedProperty);
                            value = initFunctions.GetEntityFromAddress(address);
                            break;
                        case Core.PropertyInfoType.EntityLink:
                            value = initFunctions.MakeEntityLink(DataSetUtils.GetStaticArrayPropertyValue<Core.EntityLink>(loadedProperty));
                            break;
                        case Core.PropertyInfoType.PropertyInfo:
                            Debug.LogError("Property type PropertyInfo is not supported. I don't think it even exists. Is the DataSet file well-formed?");
                            break;
                        case Core.PropertyInfoType.WideVector3:
                            Debug.LogError("Property type WideVector3 is not supported. Is the DataSet file well-formed?");
                            break;
                    }

                    if (field.PropertyInfo.Type != Core.PropertyInfoType.FilePtr)
                    {
                        field.Field.SetValue(this, value);
                    }
                }
            }
        }

        /// <summary>
        /// This is called after importing of any number of assets is complete (when the Assets progress bar has reached the end).
        /// </summary>
        /// <param name="tryGetAsset">
        /// Function to load a newly-imported or already existing asset.
        /// </param>
        public virtual void OnAssetsImported(AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
        }

        public void OnAssetsImported(
            AssetPostprocessor.GetDataSetDelegate getDataSet,
            AssetPostprocessor.TryGetAssetDelegate tryGetAsset)
        {
            //this.OnAssetsImported(tryGetAsset);

            var entriesToRemove = new HashSet<string>();
            foreach (var entry in this.desiredFiles)
            {
                Object file;
                tryGetAsset(entry.Key, out file);

                if (file == null)
                {
                    continue;
                }
                
                // TODO: Collection types
                var field = (from type in ReflectionUtils.GetParentTypes(this.GetType(), true)
                            let candidate = type.GetField(entry.Value, BindingFlags.Instance | BindingFlags.NonPublic)
                            where candidate != null
                            select candidate).First();
                Assert.IsNotNull(field);

                var propertyInfo = field.GetCustomAttribute<PropertyInfoAttribute>();
                Assert.IsNotNull(propertyInfo);

                if (propertyInfo.Container == Core.ContainerType.StaticArray && propertyInfo.ArraySize == 1)
                {
                    field.SetValue(this, file);
                }

                entriesToRemove.Add(entry.Key);
            }

            foreach (var entryToRemove in entriesToRemove)
            {
                this.desiredFiles.Remove(entryToRemove);
            }
        }

        /// <summary>
        /// Invoked when the containing DataSet is loaded.
        /// </summary>
        public virtual void OnLoaded()
        {
        }

        /// <summary>
        /// Invoked for all Entities in a DataSet after OnLoaded() has been called for each of them.
        /// </summary>
        public virtual void PostOnLoaded()
        {
        }


        /// <summary>
        /// Invoked when the containing DataSet is unloaded.
        /// </summary>
        public virtual void OnUnloaded()
        {
        }

        /// <summary>
        /// Creates writable list of Entity static properties.
        /// </summary>
        /// <param name="getEntityAddress">
        ///     Function to get an Entity's address.
        /// </param>
        /// <param name="convertEntityLink"></param>
        /// <returns>
        /// Writable static properties.
        /// </returns>
        public virtual List<Core.PropertyInfo> MakeWritableStaticProperties(
            Func<Entity, ulong> getEntityAddress,
            Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            return new List<Core.PropertyInfo>();
        }

        /// <summary>
        /// Creates writable list of Entity dynamic properties.
        /// </summary>
        /// <param name="getEntityAddress">
        /// Function to get an Entity's address.
        /// </param>
        /// <returns>
        /// Writable dynamic properties.
        /// </returns>
        public virtual List<Core.PropertyInfo> MakeWritableDynamicProperties(Func<Entity, ulong> getEntityAddress)
        {
            return new List<Core.PropertyInfo>();
        }

        /// <summary>
        /// Initializes a property from data loaded from a DataSet file.
        /// </summary>
        /// <param name="propertyData">
        /// The property data.
        /// </param>
        /// <param name="initFunctions">
        /// Initialization functions.
        /// </param>
        protected virtual void ReadProperty(Core.PropertyInfo propertyData, EntityFactory.EntityInitializeFunctions initFunctions)
        {
        }
        
        [ExposeMethodToLua(MethodStaticity.Instance)]
        public int GetClassName(lua_State L)
        {
            lua_pushstring(L, this.GetType().Name);
            return 1;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: 0x{this.GetHashCode():X}";
        }
    }
}