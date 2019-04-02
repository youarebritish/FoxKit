namespace FoxKit.Modules.DataSet.Fox.FoxCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using FoxKit.Modules.DataSet.Exporter;
    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.Importer;
    using FoxKit.Utils;
    using FoxKit.Utils.Structs;

    using FoxLib;

    using OdinSerializer;

    using UnityEditor;

    using UnityEngine;
    using UnityEngine.Assertions;

    using AssetPostprocessor = FoxKit.Core.AssetPostprocessor;
    using static KopiLua.Lua;

    using Object = UnityEngine.Object;

    public partial class Entity
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
        private Dictionary<FilePtrEntry, string> desiredFiles = new Dictionary<FilePtrEntry, string>();

        [OdinSerialize]
        private List<EntityLink> entityLinks = new List<EntityLink>();

        /// <summary>
        /// The address of the Entity in its owning DataSet.
        /// </summary>
        [OdinSerialize]
        private ulong address;

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
        /// Category of the class.
        /// </summary>
        public virtual string Category => string.Empty;

        /// <summary>
        /// Gets or sets the address of the Entity in its owning DataSet.
        /// </summary>
        public ulong Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }
        
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
            this.Address = entityData.Address;
            this.ReadStaticProperties(entityData.StaticProperties, initFunctions);

            foreach (var unused in entityData.DynamicProperties)
            {
                Debug.LogError($"Attempted to read dynamic property in an entity of type {entityData.ClassName} but dynamic properties are not yet supported. Report this.");
            }

            this.OnPropertiesLoaded();
        }

        /// <summary>
        /// Invoked after an imported Entity's properties are loaded.
        /// </summary>
        protected virtual void OnPropertiesLoaded()
        {
        }

        /// <summary>
        /// Invoked just before an Entity's properties are exported.
        /// </summary>
        public virtual void OnPreparingToExport()
        {
        }

        /// <summary>
        /// Invoked just after an Entity's properties are exported.
        /// </summary>
        public virtual void OnFinishedExporting()
        {
        }

        private object ExtractPropertyValue<TRaw, TConverted>(Core.PropertyInfo property, Func<TRaw, TConverted> conversionFunc, Core.ContainerType containerType, uint arraySize)
        {
            // Non-array
            if (containerType == Core.ContainerType.StaticArray && arraySize == 1)
            {
                var rawValue = DataSetUtils.GetStaticArrayPropertyValue<TRaw>(property);
                
                if (conversionFunc == null)
                {
                    return rawValue;
                }

                var convertedValue = conversionFunc(rawValue);

                if (property.Type == Core.PropertyInfoType.EntityLink)
                {
                    // TODO DataIdentifiers
                    var link = convertedValue as EntityLink;
                    if (link.Entity == null)
                    {
                        this.entityLinks.Add(link);
                    }
                }
                if (property.Type == Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    this.desiredFiles.Add(new FilePtrEntry(property.Name, null), convertedValue as string);
                }

                return convertedValue;
            }

            // Array
            if (containerType == Core.ContainerType.StaticArray)
            {
                var rawValues = DataSetUtils.GetStaticArrayValues<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }
                
                var convertedValues = (from rawValue in rawValues
                                      select conversionFunc(rawValue)).ToList();

                if (property.Type == Core.PropertyInfoType.EntityLink)
                {
                    foreach (var convertedValue in convertedValues)
                    {
                        // TODO DataIdentifiers
                        var link = convertedValue as EntityLink;
                        if (link.Entity == null)
                        {
                            this.entityLinks.Add(link);
                        }
                    }

                    return convertedValues;
                }
                
                if (property.Type != Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(new FilePtrEntry(property.Name, i), convertedValues[i] as string);
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

                if (property.Type == Core.PropertyInfoType.EntityLink)
                {
                    foreach (var convertedValue in convertedValues)
                    {
                        // TODO DataIdentifiers
                        var link = convertedValue as EntityLink;
                        if (link.Entity == null)
                        {
                            this.entityLinks.Add(link);
                        }
                    }

                    return convertedValues;
                }

                if (property.Type != Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(new FilePtrEntry(property.Name, i), convertedValues[i] as string);
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

                if (property.Type == Core.PropertyInfoType.EntityLink)
                {
                    foreach (var convertedValue in convertedValues)
                    {
                        // TODO DataIdentifiers
                        var link = convertedValue as EntityLink;
                        if (link.Entity == null)
                        {
                            this.entityLinks.Add(link);
                        }
                    }

                    return convertedValues;
                }

                if (property.Type != Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    return convertedValues;
                }

                for (var i = 0; i < convertedValues.Count; i++)
                {
                    this.desiredFiles.Add(new FilePtrEntry(property.Name, i), convertedValues[i] as string);
                }

                return convertedValues;
            }

            // StringMap
            if (containerType == Core.ContainerType.StringMap)
            {
                var rawValues = DataSetUtils.GetStringMap<TRaw>(property);

                if (conversionFunc == null)
                {
                    return rawValues;
                }

                var convertedValues = rawValues.ToDictionary(item => item.Key, item => conversionFunc(item.Value));

                if (property.Type == Core.PropertyInfoType.EntityLink)
                {
                    foreach (var convertedValue in convertedValues)
                    {
                        // TODO DataIdentifiers
                        var link = convertedValue.Value as EntityLink;
                        if (link.Entity == null)
                        {
                            this.entityLinks.Add(link);
                        }
                    }

                    return convertedValues;
                }

                if (property.Type != Core.PropertyInfoType.FilePtr || property.Type == Core.PropertyInfoType.Path)
                {
                    return convertedValues;
                }

                foreach (var kvp in convertedValues)
                {
                    this.desiredFiles.Add(new FilePtrEntry(property.Name, kvp.Key), kvp.Value as string);
                }

                return convertedValues;
            }

            Assert.IsTrue(false, $"Unrecognized containerType {containerType}");
            return null;
        }

        private void ReadStaticProperties(Core.PropertyInfo[] properties, EntityFactory.EntityInitializeFunctions initFunctions)
        {
            var baseTypes = ReflectionUtils.GetParentTypes(this.GetType(), true);
            var fields = (from type in baseTypes
                         from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                         let attribute = field.GetCustomAttribute<PropertyInfoAttribute>()
                         where attribute != null && !attribute.IsAutoProperty
                         select new { Field = field, PropertyInfo = attribute }).ToList();
            
            foreach (var field in fields)
            {
                var loadedProperty = properties.FirstOrDefault(property => property.Name == field.Field.Name);
                
                if (loadedProperty == null)
                {
                    Debug.LogError("Property " + field.Field.Name + " is null.");
                    continue;
                }

                Assert.IsNotNull(loadedProperty);

                var propertyInfo = field.PropertyInfo;

                object value = null;
                switch (propertyInfo.Type)
                {
                    case Core.PropertyInfoType.Int8:
                        value = ExtractPropertyValue<sbyte, sbyte>(
                            loadedProperty,
                            null,
                            propertyInfo.Container,
                            propertyInfo.ArraySize);
                        break;
                    case Core.PropertyInfoType.UInt8:
                        value = ExtractPropertyValue<byte, byte>(
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
                        /* Abandon all hope ye who enter
                        * So what's going on here is that EntityPtr properties get Entities of varying types depending on their ptrType.
                        * However, we don't know that type at compile time, so we can't call ExtractPropertyValue.
                        * To do that, we must use reflection to dynamically invoke ExtractPropertyValue with the ptrType as a generic type argument.
                        * To make matters worse, it requires a delegate as a parameter, and we need to dynamically create that delegate too since
                        * its parameters are also unknown at compile time. We use the C# Expression API to create and compile a lambda expression at runtime.
                        * 
                        * Last chance to turn back.
                        */
                        var method = typeof(Entity).GetMethod(nameof(this.ExtractPropertyValue), BindingFlags.NonPublic | BindingFlags.Instance);
                        var generic = method.MakeGenericMethod(typeof(ulong), propertyInfo.PtrType);

                        var addressParameter = Expression.Parameter(typeof(ulong), "address");
                        var initFunctionsInstance = Expression.Constant(initFunctions.GetEntityFromAddress.Target);
                        var getEntityFunction = initFunctions.GetEntityFromAddress.Method;
                        var callGetEntityFunction = Expression.Call(
                            initFunctionsInstance,
                            getEntityFunction,
                            addressParameter);
                        var cast = Expression.Convert(callGetEntityFunction, propertyInfo.PtrType);
                        var del = Expression.Lambda(cast, addressParameter).Compile();
                        
                        value = generic.Invoke(
                            this,
                            new object[]
                                {
                                    loadedProperty, del, propertyInfo.Container, propertyInfo.ArraySize 
                                });
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
                        Debug.LogError(
                            "Property type PropertyInfo is not supported. I don't think it even exists. Is the DataSet file well-formed?");
                        break;
                    case Core.PropertyInfoType.WideVector3:
                        Debug.LogError("Property type WideVector3 is not supported. Is the DataSet file well-formed?");
                        break;
                }

                // If we're dealing with an enum, we need to cast to the enum type.
                if (field.PropertyInfo.Enum != null)
                {
                    if (field.PropertyInfo.Container == Core.ContainerType.StaticArray
                        && field.PropertyInfo.ArraySize > 1)
                    {
                        var typeOfList = typeof(List<>).MakeGenericType(field.PropertyInfo.Enum);
                        var castedList = Activator.CreateInstance(typeOfList) as IList;
                        foreach (var item in (value as IList))
                        {
                            castedList.Add(item);
                        }

                        value = castedList;
                    }
                    else if (field.PropertyInfo.Container == Core.ContainerType.DynamicArray || field.PropertyInfo.Container == Core.ContainerType.List)
                    {
                        var typeOfList = typeof(List<>).MakeGenericType(field.PropertyInfo.Enum);
                        var castedList = Activator.CreateInstance(typeOfList) as IList;
                        foreach (var item in (value as IList))
                        {
                            castedList.Add(item);
                        }

                        value = castedList;
                    }
                    else if (field.PropertyInfo.Container == Core.ContainerType.StringMap)
                    {
                        var typeOfList = typeof(Dictionary<,>).MakeGenericType(typeof(string), field.PropertyInfo.Enum);
                        var oldDictionary = value as IDictionary;
                        var castedDictionary = Activator.CreateInstance(typeOfList) as IDictionary;

                        foreach (var item in oldDictionary.Keys)
                        {
                            castedDictionary.Add(item, oldDictionary[item]);
                        }

                        value = castedDictionary;
                    }
                }
                
                // FilePtr and Path properties are unique in that we don't actually get the value at this stage.
                if (field.PropertyInfo.Type != Core.PropertyInfoType.FilePtr
                    && field.PropertyInfo.Type != Core.PropertyInfoType.Path)
                {
                    field.Field.SetValue(this, value);
                }
            }
        }
        
        public void OnAssetsImported(
            AssetPostprocessor.GetDataSetDelegate getDataSet,
            AssetPostprocessor.TryGetAssetDelegate tryGetAsset,
            AssetPostprocessor.GetDataIdentifierDelegate getDataIdentifier)
        {
            // Resolve EntityLinks.
            var linksToRemove = new HashSet<EntityLink>();
            foreach (var link in this.entityLinks)
            {
                if (link.IsDataIdentifierEntityLink)
                {
                    var dataIdentifier = getDataIdentifier(link.ArchivePath);
                    if (dataIdentifier == null) continue;
                    link.SetDataIdentifier(dataIdentifier, link.NameInArchive);
                    continue;
                }

                if (string.IsNullOrEmpty(link.NameInArchive))
                {
                    if (link.Address == 0)
                    {
                        // If we have neither a name nor an address, there's nothing we can do but ignore it.
                        linksToRemove.Add(link);
                        continue;
                    }

                    Debug.LogError($"EntityLink in {this} has an address but no nameInArchive. Report this.");
                    continue;
                }
                else
                {
                    var dataSet = getDataSet(Path.GetFileName(link.ArchivePath));
                    if (dataSet == null)
                    {
                        continue;
                    }

                    var entity = dataSet.GetData(link.NameInArchive);
                    link.Entity = entity;
                    linksToRemove.Add(link);
                }
            }

            foreach (var entryToRemove in linksToRemove)
            {
                this.entityLinks.Remove(entryToRemove);
            }

            // Resolve FilePtrs/Paths.
            var entriesToRemove = new HashSet<FilePtrEntry>();
            foreach (var entry in this.desiredFiles)
            {
                Object file;
                tryGetAsset(entry.Value, out file);

                if (file == null)
                {
                    continue;
                }
                
                var field = (from type in ReflectionUtils.GetParentTypes(this.GetType(), true)
                            let candidate = type.GetField(entry.Key.FieldName, BindingFlags.Instance | BindingFlags.NonPublic)
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
                    list[(int)entry.Key.Index] = file;
                }
                else
                {
                    // StringMap
                    var dictionary = field.GetValue(this) as IDictionary;
                    Assert.IsNotNull(dictionary);
                    Assert.IsTrue(entry.Key.Index is string);
                    dictionary[entry.Key.Index] = file;
                }

                entriesToRemove.Add(entry.Key);
            }

            foreach (var entryToRemove in entriesToRemove)
            {
                this.desiredFiles.Remove(entryToRemove);
            }
        }

        public delegate SceneProxy CreateSceneProxyDelegate();

        /// <summary>
        /// Invoked when the containing DataSet is loaded.
        /// </summary>
        public virtual void OnLoaded(CreateSceneProxyDelegate createSceneProxy)
        {
        }

        public delegate SceneProxy GetSceneProxyDelegate(string entityName);

        /// <summary>
        /// Invoked for all Entities in a DataSet after OnLoaded() has been called for each of them.
        /// </summary>
        public virtual void PostOnLoaded(GetSceneProxyDelegate getSceneProxy)
        {
        }


        public delegate void DestroySceneProxyDelegate();

        /// <summary>
        /// Invoked when the containing DataSet is unloaded.
        /// </summary>
        public virtual void OnUnloaded(DestroySceneProxyDelegate destroySceneProxy)
        {
        }
        
        /// <summary>
        /// Creates writable list of Entity static properties.
        /// </summary>
        /// <param name="getEntityAddress">
        /// Function to get an Entity's address.
        /// </param>
        /// <param name="convertEntityLink"></param>
        /// <returns>
        /// Writable static properties.
        /// </returns>
        public virtual IEnumerable<Core.PropertyInfo> MakeWritableStaticProperties(Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            return from type in ReflectionUtils.GetParentTypes(this.GetType(), true)
                   from field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                   let attribute = field.GetCustomAttribute<PropertyInfoAttribute>()
                   where attribute != null && !attribute.IsAutoProperty
                   select this.MakeWritableStaticProperty(field, attribute, getEntityAddress, convertEntityLink);
        }

        private Core.PropertyInfo MakeWritableStaticProperty(FieldInfo field, PropertyInfoAttribute attribute, Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink)
        {
            if (attribute.Container == Core.ContainerType.StaticArray && attribute.ArraySize == 1)
            {
                return PropertyInfoFactory.MakeStaticArrayProperty(field.Name, attribute.Type, ConvertValueToFox(attribute.Type, getEntityAddress, convertEntityLink, field.GetValue(this)));
            }
            if (attribute.Container == Core.ContainerType.StaticArray)
            {
                var values = from value in (field.GetValue(this) as IList).Cast<object>()
                             select ConvertValueToFox(attribute.Type, getEntityAddress, convertEntityLink, value);
                return PropertyInfoFactory.MakeStaticArrayProperty(field.Name, attribute.Type, values.ToArray());
            }
            if (attribute.Container == Core.ContainerType.DynamicArray)
            {
                var values = from value in (field.GetValue(this) as IList).Cast<object>()
                             select ConvertValueToFox(attribute.Type, getEntityAddress, convertEntityLink, value);
                return PropertyInfoFactory.MakeDynamicArrayProperty(field.Name, attribute.Type, values.ToArray());
            }
            if (attribute.Container == Core.ContainerType.List)
            {
                var values = from value in (field.GetValue(this) as IList).Cast<object>()
                             select ConvertValueToFox(attribute.Type, getEntityAddress, convertEntityLink, value);
                return PropertyInfoFactory.MakeListProperty(field.Name, attribute.Type, values.ToArray());
            }
            if (attribute.Container == Core.ContainerType.StringMap)
            {
                var enumerableDictionary = IDictionaryToIEnumerable(field.GetValue(this) as IDictionary);
                var dict = enumerableDictionary.ToDictionary(
                    entry => entry.Key as string,
                    entry => ConvertValueToFox(attribute.Type, getEntityAddress, convertEntityLink, entry.Value));
                return PropertyInfoFactory.MakeStringMapProperty(field.Name, attribute.Type, dict);
            }
            Assert.IsTrue(false, "Invalid container.");
            return null;
        }

        private static IEnumerable<DictionaryEntry> IDictionaryToIEnumerable(IDictionary dict)
        {
            // Note: Resharper thinks this can be converted into a call to dict.Cast<DictionaryEntry>(), but this is incorrect.
            foreach (var item in dict)
            {
                yield return (DictionaryEntry)item;
            }
        }

        private static object ConvertValueToFox(Core.PropertyInfoType type, Func<Entity, ulong> getEntityAddress, Func<EntityLink, Core.EntityLink> convertEntityLink, object value)
        {
            switch (type)
            {
                case Core.PropertyInfoType.Int8:
                    return value;
                case Core.PropertyInfoType.UInt8:
                    return value;
                case Core.PropertyInfoType.Int16:
                    return value;
                case Core.PropertyInfoType.UInt16:
                    return value;
                case Core.PropertyInfoType.Int32:
                    return value;
                case Core.PropertyInfoType.UInt32:
                    return value;
                case Core.PropertyInfoType.Int64:
                    return value;
                case Core.PropertyInfoType.UInt64:
                    return value;
                case Core.PropertyInfoType.Float:
                    return value;
                case Core.PropertyInfoType.Double:
                    return value;
                case Core.PropertyInfoType.Bool:
                    return value;
                case Core.PropertyInfoType.String:
                    return value;
                case Core.PropertyInfoType.Path:
                    return FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(value as UnityEngine.Object));
                case Core.PropertyInfoType.EntityPtr:
                    return getEntityAddress(value as Entity);
                case Core.PropertyInfoType.Vector3:
                    return FoxUtils.UnityToFox((UnityEngine.Vector3)value);
                case Core.PropertyInfoType.Vector4:
                    return FoxUtils.UnityToFox((UnityEngine.Vector4)value);
                case Core.PropertyInfoType.Quat:
                    return FoxUtils.UnityToFox((UnityEngine.Quaternion)value);
                case Core.PropertyInfoType.Matrix3:
                    return FoxUtils.UnityToFox((Matrix3x3)value);
                case Core.PropertyInfoType.Matrix4:
                    return FoxUtils.UnityToFox((Matrix4x4)value);
                case Core.PropertyInfoType.Color:
                    return FoxUtils.UnityColorToFoxColorRGBA((Color)value);
                case Core.PropertyInfoType.FilePtr:
                    return FoxUtils.UnityPathToFoxPath(AssetDatabase.GetAssetPath(value as UnityEngine.Object));
                case Core.PropertyInfoType.EntityHandle:
                    return getEntityAddress(value as Entity);
                case Core.PropertyInfoType.EntityLink:
                    return convertEntityLink(value as EntityLink);
                case Core.PropertyInfoType.PropertyInfo:
                    Assert.IsTrue(false, "Attempting to convert value of type PropertyInfo. This should never happen.");
                    break;
                case Core.PropertyInfoType.WideVector3:
                    Assert.IsTrue(false, "Attempting to convert value of type WideVector3. This should never happen.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return null;
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
        
        partial void GetClassName(lua_State L)
        {
            lua_pushstring(L, this.GetType().Name);
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}: 0x{this.GetHashCode():X}";
        }
    }
}