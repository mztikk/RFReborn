using System;
using System.ComponentModel;
using System.Reflection;

namespace RFReborn.Internals;

/// <summary>
/// Class that provides methods to manipulate properties at runtime dynamically by name
/// </summary>
[Obsolete("With NET5 there will be a source generator equivalent", false)]
public static class DynamicProperty
{
    /// <summary>
    /// Converts the string <paramref name="obj"/> to the type <paramref name="type"/>
    /// </summary>
    /// <param name="type"><see cref="Type"/> to convert to</param>
    /// <param name="obj">String representation to convert</param>
    public static object ConvertValue(Type type, string obj)
    {
        // check enum first
        // otherwise underlying type of enum will be checked in typecode
        if (type.IsEnum)
        {
            return Enum.Parse(type, obj);
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                return bool.Parse(obj);
            case TypeCode.Char:
                return char.Parse(obj);
            case TypeCode.SByte:
                return sbyte.Parse(obj);
            case TypeCode.Byte:
                return byte.Parse(obj);
            case TypeCode.Int16:
                return short.Parse(obj);
            case TypeCode.UInt16:
                return ushort.Parse(obj);
            case TypeCode.Int32:
                return int.Parse(obj);
            case TypeCode.UInt32:
                return uint.Parse(obj);
            case TypeCode.Int64:
                return long.Parse(obj);
            case TypeCode.UInt64:
                return ulong.Parse(obj);
            case TypeCode.Single:
                return float.Parse(obj);
            case TypeCode.Double:
                return double.Parse(obj);
            case TypeCode.Decimal:
                return decimal.Parse(obj);
            case TypeCode.DateTime:
                return DateTime.Parse(obj);
            case TypeCode.String:
                return obj;
        }

        TypeConverter converter = TypeDescriptor.GetConverter(type);
        if (converter != null)
        {
            return converter.ConvertFrom(obj);
        }

        return obj;
    }

    /// <summary>
    /// Sets the value of a property by name
    /// </summary>
    /// <param name="t">Type of object</param>
    /// <param name="obj">Object that has property</param>
    /// <param name="value">Value to set</param>
    /// <param name="nameChain">Names of properties to walk</param>
    /// <param name="index">index to start in <paramref name="nameChain"/>, default 0</param>
    public static void Set(Type t, object obj, string value, string[] nameChain, int index = 0)
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        string last = nameChain[^1];
        for (int i = index; i < nameChain.Length; i++)
        {
            foreach (PropertyInfo prop in t.GetProperties(bindingFlags))
            {
                string name = nameChain[i];

                if (prop.Name != name)
                {
                    continue;
                }

                if (name == last)
                {
                    object tValue = ConvertValue(prop.PropertyType, value);
                    prop.SetValue(obj, tValue);
                }
                else
                {
                    Set(prop.PropertyType, prop.GetValue(obj), value, nameChain, ++i);
                }
            }
        }
    }

    /// <summary>
    /// Sets the value of a property by name
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="obj">Object where to set the property</param>
    /// <param name="name">Name of property</param>
    /// <param name="value">Value to set</param>
    public static void Set<T>(T obj, string name, string value)
    {
        Type t = typeof(T);
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        foreach (PropertyInfo prop in t.GetProperties(bindingFlags))
        {
            if (prop.Name != name)
            {
                continue;
            }

            object tValue = ConvertValue(prop.PropertyType, value);
            prop.SetValue(obj, tValue);
        }
    }

    /// <summary>
    /// Returns a property by name
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="obj">Object that has the property</param>
    /// <param name="name">Name of property</param>
    public static object Get<T>(T obj, string name)
    {
        Type t = typeof(T);
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        foreach (PropertyInfo prop in t.GetProperties(bindingFlags))
        {
            if (prop.Name == name)
            {
                return prop.GetValue(obj);
            }
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Gets a property by name
    /// </summary>
    /// <param name="t">Type of object</param>
    /// <param name="nameChain">Names of properties to walk</param>
    /// <param name="index">index to start in <paramref name="nameChain"/>, default 0</param>
    public static PropertyInfo Get(Type t, string[] nameChain, int index = 0)
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        string last = nameChain[^1];
        for (int i = index; i < nameChain.Length; i++)
        {
            foreach (PropertyInfo prop in t.GetProperties(bindingFlags))
            {
                string name = nameChain[i];

                if (prop.Name != name)
                {
                    continue;
                }

                if (name == last)
                {
                    return prop;
                }
                else
                {
                    return Get(prop.PropertyType, nameChain, ++i);
                }
            }
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Gets a properties value by name
    /// </summary>
    /// <param name="t">Type of object</param>
    /// <param name="obj">Object that has property</param>
    /// <param name="nameChain">Names of properties to walk</param>
    /// <param name="index">index to start in <paramref name="nameChain"/>, default 0</param>
    public static object Get(Type t, object obj, string[] nameChain, int index = 0)
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

        string last = nameChain[^1];
        for (int i = index; i < nameChain.Length; i++)
        {
            foreach (PropertyInfo prop in t.GetProperties(bindingFlags))
            {
                string name = nameChain[i];

                if (prop.Name != name)
                {
                    continue;
                }

                if (name == last)
                {
                    return prop.GetValue(obj);
                }
                else
                {
                    return Get(prop.PropertyType, prop.GetValue(obj), nameChain, ++i);
                }
            }
        }

        throw new ArgumentException();
    }
}
