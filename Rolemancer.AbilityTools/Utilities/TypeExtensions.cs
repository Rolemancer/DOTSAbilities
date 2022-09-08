using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rolemancer.AbilityTools.Utilities
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllDerivedTypes<T>()
        {
            var type = typeof(T);
            return Assembly.GetAssembly(type).GetAllDerivedTypes(type);
        }
        
        public static IEnumerable<Type> GetAllDerivedTypes(this Type type)
        {
            return Assembly.GetAssembly(type).GetAllDerivedTypes(type);
        }
        
        private static IEnumerable<Type> GetAllDerivedTypes(this Assembly assembly, Type type)
        {
            return assembly
                .GetTypes()
                .Where(t => t != type && type.IsAssignableFrom(t))
                .ToList();
        }
    }
}