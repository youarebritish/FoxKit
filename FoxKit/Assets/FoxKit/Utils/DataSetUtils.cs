namespace FoxKit.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;
    using FoxKit.Modules.DataSet.Importer;

    using FoxLib;

    using UnityEditor;

    using UnityEngine.Assertions;

    /// <summary>
    /// Helper functions for working with DataSets.
    /// </summary>
    public static class DataSetUtils
    {
        /// <summary>
        /// Get the single value in a StaticArray property with exactly one element.
        /// </summary>
        /// <typeparam name="TValue">Type of the value to get.</typeparam>
        /// <param name="property">The property whose value to get.</param>
        /// <returns>The extracted value.</returns>
        public static TValue GetStaticArrayPropertyValue<TValue>(Core.PropertyInfo property)
        {
            CheckContainerType(property, Core.ContainerType.StaticArray);

            var container = ((Core.Container<TValue>.StaticArray)property.Container).Item;
            Assert.IsTrue(
                container.Length == 1,
                $"Expected a StaticArray containing exactly one element, but found one with {container.Length} in property {property.Name}.");

            return container[0];
        }

        public static List<TValue> GetStaticArrayValues<TValue>(Core.PropertyInfo property)
        {
            CheckContainerType(property, Core.ContainerType.StaticArray);
            return ((Core.Container<TValue>.StaticArray)property.Container).Item.ToList();
        }

        public static List<TValue> GetDynamicArrayValues<TValue>(Core.PropertyInfo property)
        {
            CheckContainerType(property, Core.ContainerType.DynamicArray);
            return ((Core.Container<TValue>.DynamicArray)property.Container).Item.ToList();
        }

        public static List<TValue> GetListValues<TValue>(Core.PropertyInfo property)
        {
            CheckContainerType(property, Core.ContainerType.List);
            return ((Core.Container<TValue>.List)property.Container).Item.ToList();
        }

        public static Dictionary<string, TValue> GetStringMap<TValue>(Core.PropertyInfo property)
        {
            CheckContainerType(property, Core.ContainerType.StringMap);
            var container = (Core.Container<TValue>.StringMap)property.Container;
            return container.Item.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static string AssetToFoxPath(UnityEngine.Object asset)
        {
            var unityPath = AssetDatabase.GetAssetPath(asset);
            return FoxUtils.UnityPathToFoxPath(unityPath);
        }

        public static EntityLink MakeEntityLink(DataSet owningDataSet, Core.EntityLink foxEntityLink, EntityFactory.EntityInitializeFunctions.GetEntityFromAddressDelegate getEntityByAddress, Func<string, Data> getEntityByName)
        {
            var link = new EntityLink(
                foxEntityLink.PackagePath,
                foxEntityLink.ArchivePath,
                foxEntityLink.NameInArchive,
                foxEntityLink.EntityHandle);

            // Store the archivePath for convenience later.
            if (!link.IsDataIdentifierEntityLink)
            {
                link.ArchivePath = AssetDatabase.GUIDToAssetPath(owningDataSet.DataSetGuid);

                // If the EntityLink references an Entity inside its own DataSet, resolve it now.
                if (Path.GetFileNameWithoutExtension(link.ArchivePath) == owningDataSet.OwningDataSetName)
                {
                    if (link.Address != 0)
                    {
                        link.Entity = getEntityByAddress(link.Address) as Data;
                    }
                    else if (!string.IsNullOrEmpty(link.NameInArchive))
                    {
                        link.Entity = getEntityByName(link.NameInArchive);
                    }
                }
            }

            return link;
        }

        public static Core.EntityLink MakeEntityLink(EntityLink unityEntityLink)
        {
            return new Core.EntityLink(
                unityEntityLink.PackagePath,
                unityEntityLink.ArchivePath,
                unityEntityLink.NameInArchive,
                unityEntityLink.Address);
        }

        /// <summary>
        /// Does the given type require conversion to a Unity type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool DoesTypeRequireConversion(Core.PropertyInfoType type)
        {
            switch(type)
            {
                case Core.PropertyInfoType.Path:
                    return true;
                case Core.PropertyInfoType.EntityPtr:
                    return true;
                case Core.PropertyInfoType.Vector3:
                    return true;
                case Core.PropertyInfoType.Vector4:
                    return true;
                case Core.PropertyInfoType.Quat:
                    return true;
                case Core.PropertyInfoType.Matrix3:
                    return true;
                case Core.PropertyInfoType.Matrix4:
                    return true;
                case Core.PropertyInfoType.Color:
                    return true;
                case Core.PropertyInfoType.FilePtr:
                    return true;
                case Core.PropertyInfoType.EntityHandle:
                    return true;
                case Core.PropertyInfoType.EntityLink:
                    return true;
                case Core.PropertyInfoType.WideVector3:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Is the given type a reference type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeReference(Core.PropertyInfoType type)
        {
            switch (type)
            {
                case Core.PropertyInfoType.FilePtr:
                    return true;
                case Core.PropertyInfoType.EntityHandle:
                    return true;
                case Core.PropertyInfoType.EntityLink:
                    return true;
                case Core.PropertyInfoType.EntityPtr:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Asserts that a property has a container of a given type.
        /// </summary>
        /// <param name="property">The property whose type to check.</param>
        /// <param name="expectedContainerType">The expected container type.</param>
        private static void CheckContainerType(Core.PropertyInfo property, Core.ContainerType expectedContainerType)
        {
            Assert.IsTrue(
                property.ContainerType == expectedContainerType,
                $"Expected container type {expectedContainerType} but found {property.ContainerType} in property {property.Name}.");
        }
    }
}