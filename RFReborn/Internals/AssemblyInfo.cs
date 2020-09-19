using System;
using System.Collections.Generic;
using System.Reflection;
using RFReborn.Extensions;

namespace RFReborn.Internals
{
    /// <summary>
    /// Helper class that provides Information about <see cref="Assembly"/>s;
    /// </summary>
    public static class AssemblyInfo
    {
        private static Assembly[] GetAllAssemblies() => AppDomain.CurrentDomain.GetAssemblies();

        /// <summary>
        /// Enumerates all <see cref="Type"/>s currently exported
        /// </summary>
        public static IEnumerable<Type> GetAllTypes()
        {
            foreach (Assembly assembly in GetAllAssemblies())
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Enumerates all <see cref="Type"/>s that derive from or are a given <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType">Base <see cref="Type"/> matching types need to derive from</param>
        public static IEnumerable<Type> GetAllTypes(Type baseType)
        {
            foreach (Type type in GetAllTypes())
            {
                if (baseType.IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Enumerates all <see cref="Type"/>s that derive from or are of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Base <see cref="Type"/> matching types need to derive from</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllTypes<T>() => GetAllTypes(typeof(T));

        /// <summary>
        /// Enumerates all <see cref="Type"/>s that implement a given <see langword="interface"/> <paramref name="baseType"/>
        /// </summary>
        /// <param name="baseType">Base <see cref="Type"/> matching types need to derive from</param>
        public static IEnumerable<Type> GetAllInterfaceImplementations(Type baseType)
        {
            if (!baseType.IsInterface)
            {
                throw new ArgumentException($"{nameof(baseType)} has to be an interface");
            }

            foreach (Type type in GetAllTypes())
            {
                Type[] interfaces = type.GetInterfaces();
                if (interfaces.Any(x => x == baseType))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Enumerates all <see cref="Type"/>s that implement a given <see langword="interface"/> <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Base <see cref="Type"/> matching types need to derive from</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllInterfaceImplementations<T>() => GetAllInterfaceImplementations(typeof(T));
    }
}
