using System;
using System.Collections.Generic;
using System.Reflection;
using RFReborn.Extensions;

namespace RFReborn.Internals
{
    /// <summary>
    /// Helper class that provides Information about <see cref="Assembly"/>s;
    /// </summary>
    [Obsolete("Reflection API soon to be obsoleted in RFReborn in favor of source generator equivalents", false)]
    public static class AssemblyInfo
    {
        private static Assembly[] GetAllAssemblies() => AppDomain.CurrentDomain.GetAssemblies();

        /// <summary>
        /// Enumerates all <see cref="Type"/>s exported in the given <paramref name="assemblies"/>
        /// </summary>
        public static IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Enumerates all <see cref="Type"/>s currently exported
        /// </summary>
        public static IEnumerable<Type> GetAllTypes() => GetAllTypes(GetAllAssemblies());

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

            return GetAllInterfaceImplementationsInternal();

            IEnumerable<Type> GetAllInterfaceImplementationsInternal()
            {
                foreach (Type type in GetAllTypes())
                {
                    Type[] interfaces = type.GetInterfaces();
                    if (interfaces.Any(x => x == baseType))
                    {
                        yield return type;
                    }
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
