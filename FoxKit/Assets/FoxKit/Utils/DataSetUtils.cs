namespace FoxKit.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using FoxKit.Modules.DataSet.FoxCore;

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

        public static string UnityPathToFoxPath(string filePtr)
        {
            if (string.IsNullOrEmpty(filePtr))
            {
                return string.Empty;
            }

            // Fox Engine paths need to open with a /.
            return "/" + filePtr;
        }

        public static string AssetToFoxPath(UnityEngine.Object asset)
        {
            var unityPath = AssetDatabase.GetAssetPath(asset);
            return UnityPathToFoxPath(unityPath);
        }

        public static string ExtractFilePath(string filePtr)
        {
            return FormatFilePath(filePtr);
        }
        
        private static string FormatFilePath(string path)
        {
            // Fox Engine paths open with a /, which Unity doesn't like.
            return string.IsNullOrEmpty(path) ? path : path.Substring(1);
        }

        public static EntityLink MakeEntityLink(DataSet owningDataSet, Core.EntityLink foxEntityLink)
        {
            return new EntityLink(
                owningDataSet,
                foxEntityLink.PackagePath,
                foxEntityLink.ArchivePath,
                foxEntityLink.NameInArchive,
                foxEntityLink.EntityHandle);
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