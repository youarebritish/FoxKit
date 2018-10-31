namespace FoxKit.Utils
{
    using System.Collections.Generic;
    using System;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// Collection of utility functions for working with reflection.
    /// </summary>
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetAssignableConcreteClasses(Type baseType)
        {
            return from type in Assembly.GetAssembly(baseType).GetTypes()
                   where baseType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
                   select type;
        }

        /// <summary>
        /// Gets all parent types of a given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeSelfType"></param>
        /// <returns></returns>
        public static IList<Type> GetParentTypes(Type type, bool includeSelfType = false)
        {
            var baseTypes = new List<Type>();
            if (includeSelfType)
            {
                baseTypes.Add(type);
            }

            var baseType = type.BaseType;
            while (baseType != null)
            {
                baseTypes.Add(baseType);
                baseType = baseType.BaseType;
            }
            return baseTypes;
        }
    }
}