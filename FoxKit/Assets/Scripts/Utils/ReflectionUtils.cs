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
            /*var allTypes = Assembly.GetAssembly(baseType).GetTypes();
            var assignableTypes = allTypes.Where(type => baseType.IsAssignableFrom(type)).ToList();
            var concreteTypes = assignableTypes.Where(type => type.IsClass && !type.IsAbstract).ToList();
            return concreteTypes;*/
            return from type in Assembly.GetAssembly(baseType).GetTypes()
                   where baseType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
                   select type;
        }
    }
}