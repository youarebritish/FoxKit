using FoxTool.Fox;
using FoxTool.Fox.Containers;
using FoxTool.Fox.Types;
using FoxTool.Fox.Types.Structs;
using System.Linq;
using UnityEngine.Assertions;

namespace FoxKit.Utils
{
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
        public static TValue GetStaticArrayPropertyValue<TValue>(FoxProperty property) where TValue : IFoxValue, new()
        {
            CheckContainerType(property, FoxContainerType.StaticArray);

            var container = (property.Container as FoxStaticArray<TValue>).ToList();
            Assert.IsTrue(container.Count == 1,
                $"Expected a StaticArray containing exactly one element, but found one with {container.Count} in property {property.Name}.");

            return container[0];
        }

        public static UnityEngine.Vector3 FoxToolToUnity(FoxVector3 foxVector)
        {
            return new UnityEngine.Vector3(foxVector.Z, foxVector.Y, foxVector.X);
        }

        public static UnityEngine.Quaternion FoxToolToUnity(FoxQuat foxQuat)
        {
            return new UnityEngine.Quaternion(-foxQuat.Z, -foxQuat.Y, -foxQuat.X, foxQuat.W);
        }

        /// <summary>
        /// Asserts that a property has a container of a given type.
        /// </summary>
        /// <param name="property">The property whose type to check.</param>
        /// <param name="expectedContainerType">The expected container type.</param>
        private static void CheckContainerType(FoxProperty property, FoxContainerType expectedContainerType)
        {
            Assert.IsTrue(property.ContainerType == expectedContainerType,
                $"Expected container type {expectedContainerType} but found {property.ContainerType} in property {property.Name}.");
        }
    }
}