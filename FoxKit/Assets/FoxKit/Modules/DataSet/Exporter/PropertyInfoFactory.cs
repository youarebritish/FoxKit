namespace FoxKit.Modules.DataSet.Exporter
{
    using System.Collections.Generic;

    using FoxLib;

    using UnityEngine.Assertions;

    /// <summary>
    /// Collection of helper functions for creating writable PropertyInfos.
    /// </summary>
    public static class PropertyInfoFactory
    {
        public static Core.PropertyInfo MakeStaticArrayProperty(string name, Core.PropertyInfoType propertyType, object value)
        {
            Assert.IsNotNull(value);

            var container = Core.Container<object>.NewStaticArray(new[] { value });
            return new Core.PropertyInfo(name, propertyType, Core.ContainerType.StaticArray, container);
        }

        public static Core.PropertyInfo MakeDynamicArrayProperty(string name, Core.PropertyInfoType propertyType, object[] values)
        {
            Assert.IsNotNull(values);

            var container = Core.Container<object>.NewDynamicArray(values);
            return new Core.PropertyInfo(name, propertyType, Core.ContainerType.DynamicArray, container);
        }

        public static Core.PropertyInfo MakeListProperty(string name, Core.PropertyInfoType propertyType, object[] values)
        {
            Assert.IsNotNull(values);

            var container = Core.Container<object>.NewList(values);
            return new Core.PropertyInfo(name, propertyType, Core.ContainerType.List, container);
        }

        public static Core.PropertyInfo MakeStringMapProperty(string name, Core.PropertyInfoType propertyType, IDictionary<string, object> values)
        {
            Assert.IsNotNull(values);

            var container = Core.Container<object>.NewStringMap(values);
            return new Core.PropertyInfo(name, propertyType, Core.ContainerType.StringMap, container);
        }
    }
}