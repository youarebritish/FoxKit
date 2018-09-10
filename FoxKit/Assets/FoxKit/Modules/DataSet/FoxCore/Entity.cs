﻿namespace FoxKit.Modules.DataSet.FoxCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using FoxKit.Modules.DataSet.Importer;
    using FoxKit.Modules.Lua;
    using FoxKit.Utils;
    using FoxKit.Utils.Structs;

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
        [Serializable]
        private class FilePtrEntry
        {
            /// <summary>
            /// Name of the field that wants the file.
            /// </summary>
            public readonly string FieldName;

            /// <summary>
            /// Index of the field's container into which to put the file.
            /// Should be null for a non-array property, an int for an array property, or a string for a StringMap property.
            /// </summary>
            public readonly object Index;

            public FilePtrEntry(string fieldName, object index)
            {
                this.FieldName = fieldName;
                this.Index = index;
            }
        }

        /// <summary>
        /// Files that this Entity is looking for, along with the name of the field that wants the file.
        /// </summary>
        [OdinSerialize]
        private Dictionary<string, FilePtrEntry> desiredFiles = new Dictionary<string, FilePtrEntry>();

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

        private object ExtractPropertyValue<TRaw, TConverted>(Core.PropertyInfo property, Func<TRaw, TConverted> conversionFunc, Core.ContainerType containerType, uint arraySize)
        {
            if (containerType == Core.ContainerType.StaticArray && arraySize == 1)
            {
                var rawValue = DataSetUtils.GetStaticArrayPropertyValue<TRaw>(property);
                
                if (conversionFunc == null)
                {
                    return rawValue;
                }

                var convertedValue = conversionFunc(rawValue);

                if (property.Type == Core.PropertyInfoType.FilePtr)
                {
                    this.desiredFiles.Add(convertedValue as string, new FilePtrEntry(property.Name, null));
                }

                return convertedValue;
            }
            if (containerType == Core.ContainerType.StaticArray)
            {
                var rawValues = DataSetUtils.GetStaticArrayValues<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }

                var convertedValues = (from rawValue in rawValues
                                      select conversionFunc(rawValue)).ToList();

                if (property.Type != Core.PropertyInfoType.FilePtr)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(convertedValues[i] as string, new FilePtrEntry(property.Name, i));
                }

                return convertedValues;
            }
            if (containerType == Core.ContainerType.DynamicArray)
            {
                var rawValues = DataSetUtils.GetDynamicArrayValues<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }

                var convertedValues = (from rawValue in rawValues
                                       select conversionFunc(rawValue)).ToList();

                if (property.Type != Core.PropertyInfoType.FilePtr)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(convertedValues[i] as string, new FilePtrEntry(property.Name, i));
                }

                return convertedValues;
            }
            if (containerType == Core.ContainerType.List)
            {
                var rawValues = DataSetUtils.GetListValues<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }

                var convertedValues = (from rawValue in rawValues
                                       select conversionFunc(rawValue)).ToList();

                if (property.Type != Core.PropertyInfoType.FilePtr)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(convertedValues[i] as string, new FilePtrEntry(property.Name, i));
                }

                return convertedValues;
            }
            if (containerType == Core.ContainerType.StringMap)
            {
                var rawValues = DataSetUtils.GetStringMap<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }

                var convertedValues = rawValues.ToDictionary(item => item.Key, item => conversionFunc(item.Value));

                if (property.Type != Core.PropertyInfoType.FilePtr)
                {
                    return convertedValues;
                }

                foreach (var kvp in convertedValues)
                {
                    this.desiredFiles.Add(kvp.Value as string, new FilePtrEntry(property.Name, kvp.Key));
                }
            }

            Assert.IsTrue(false, "Unrecognized containerType");
            return null;
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
                
                object value = null;
                switch (propertyInfo.Type)
                {
                    case Core.PropertyInfoType.Int8:
                        value = ExtractPropertyValue<byte, byte>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.UInt8:
                        value = ExtractPropertyValue<sbyte, sbyte>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Int16:
                        value = ExtractPropertyValue<short, short>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.UInt16:
                        value = ExtractPropertyValue<ushort, ushort>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Int32:
                        value = ExtractPropertyValue<int, int>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.UInt32:
                        value = ExtractPropertyValue<uint, uint>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Int64:
                        value = ExtractPropertyValue<long, long>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.UInt64:
                        value = ExtractPropertyValue<ulong, ulong>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Float:
                        value = ExtractPropertyValue<float, float>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Double:
                        value = ExtractPropertyValue<double, double>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Bool:
                        value = ExtractPropertyValue<bool, bool>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.String:
                        value = ExtractPropertyValue<string, string>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Path:
                        value = ExtractPropertyValue<string, string>(
                            loadedProperty,
                            FoxUtils.FoxPathToUnityPath,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.EntityPtr:
                        value = ExtractPropertyValue<ulong, Entity>(
                            loadedProperty,
                            address => initFunctions.GetEntityFromAddress(address),
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Vector3:
                        value = ExtractPropertyValue<Core.Vector3, UnityEngine.Vector3>(
                            loadedProperty,
                            FoxUtils.FoxToUnity,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Vector4:
                        value = ExtractPropertyValue<Core.Vector4, UnityEngine.Vector4>(
                            loadedProperty,
                            FoxUtils.FoxToUnity,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Quat:
                        value = ExtractPropertyValue<Core.Quaternion, UnityEngine.Quaternion>(
                            loadedProperty,
                            FoxUtils.FoxToUnity,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Matrix3:
                        value = ExtractPropertyValue<Core.Matrix3, Matrix3x3>(
                            loadedProperty,
                            FoxUtils.FoxToUnity,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Matrix4:
                        value = ExtractPropertyValue<Core.Matrix4, UnityEngine.Matrix4x4>(
                            loadedProperty,
                            FoxUtils.FoxToUnity,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.Color:
                        value = ExtractPropertyValue<Core.ColorRGBA, UnityEngine.Color>(
                            loadedProperty,
                            FoxUtils.FoxColorRGBAToUnityColor,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.FilePtr:
                        value = ExtractPropertyValue<string, string>(
                            loadedProperty,
                            FoxUtils.FoxPathToUnityPath,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.EntityHandle:
                        value = ExtractPropertyValue<ulong, Entity>(
                            loadedProperty,
                            address => initFunctions.GetEntityFromAddress(address),
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.EntityLink:
                        value = ExtractPropertyValue<Core.EntityLink, EntityLink>(
                            loadedProperty,
                            rawValue => initFunctions.MakeEntityLink(rawValue),
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.PropertyInfo:
                        Debug.LogError("Property type PropertyInfo is not supported. I don't think it even exists. Is the DataSet file well-formed?");
                        break;
                    case Core.PropertyInfoType.WideVector3:
                        Debug.LogError("Property type WideVector3 is not supported. Is the DataSet file well-formed?");
                        break;
                }

                // FilePtr properties are unique in that we don't actually get the value at this stage.
                if (field.PropertyInfo.Type != Core.PropertyInfoType.FilePtr)
                {
                    field.Field.SetValue(this, value);
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

            // TODO: This will break if multiple properties want the same file. Consider swapping the keys and the values.
            var entriesToRemove = new HashSet<string>();
            foreach (var entry in this.desiredFiles)
            {
                Object file;
                tryGetAsset(entry.Key, out file);

                if (file == null)
                {
                    continue;
                }
                
                var field = (from type in ReflectionUtils.GetParentTypes(this.GetType(), true)
                            let candidate = type.GetField(entry.Value.FieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                            where candidate != null
                            select candidate).First();
                Assert.IsNotNull(field);

                var propertyInfo = field.GetCustomAttribute<PropertyInfoAttribute>();
                Assert.IsNotNull(propertyInfo);

                if (propertyInfo.Container == Core.ContainerType.StaticArray && propertyInfo.ArraySize == 1)
                {
                    field.SetValue(this, file);
                }
                else if (propertyInfo.Container == Core.ContainerType.StaticArray || propertyInfo.Container == Core.ContainerType.DynamicArray)
                {
                    var list = field.GetValue(this) as IList;
                    Assert.IsNotNull(list);
                    list[(int)entry.Value.Index] = file;
                }
                else
                {
                    // StringMap
                    var dictionary = field.GetValue(this) as IDictionary;
                    Assert.IsNotNull(dictionary);
                    Assert.IsTrue(entry.Value.Index is string);
                    dictionary[entry.Value.Index] = file;
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