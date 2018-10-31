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
        public static IEnumerable<Type> GetParentTypes(Type type, bool includeSelfType = false)
        {
            var baseTypes = new Stack<Type>();
            if (includeSelfType)
            {
                baseTypes.Push(type);
            }

            var baseType = type.BaseType;
            while (baseType != null)
            {
                baseTypes.Push(baseType);
                baseType = baseType.BaseType;
            }

            return baseTypes;
        }
    }
}