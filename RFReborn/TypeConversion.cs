using System;

namespace RFReborn;

/// <summary>
/// Class used to convert unmanaged objects to byte arrays or from bytes to an object.
/// </summary>
/// <typeparam name="T">Type used for conversion</typeparam>
public static class TypeConversion<T> where T : unmanaged
{
    static unsafe TypeConversion()
    {
        Type = typeof(T);
        TypeCode = Type.GetTypeCode(Type);
        Size = TypeCode == TypeCode.Boolean ? 1 : sizeof(T);
    }

    /// <summary>
    /// <see cref="Type"/> of <typeparamref name="T"/>
    /// </summary>
    public static Type Type { get; }

    /// <summary>
    /// <see cref="TypeCode"/> of <typeparamref name="T"/>
    /// </summary>
    public static TypeCode TypeCode { get; }

    /// <summary>
    /// Size in bytes of <typeparamref name="T"/>
    /// </summary>
    public static int Size { get; }

    /// <summary>
    /// Converts an unmanaged object to a byte array
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>Returns an array consisting of the bytes of the <paramref name="obj"/></returns>
    public static unsafe byte[] ToByteArray(in T obj)
    {
        byte[] rtn = new byte[Size];
        fixed (void* rtnPointer = rtn)
        {
            T* a = (T*)rtnPointer;
            *a = obj;
        }

        return rtn;
    }

    /// <summary>
    /// Converts a byte array to an object of <typeparamref name="T"/>
    /// </summary>
    /// <param name="bytes">Bytes to convert</param>
    /// <param name="index">Start index inside the byte array</param>
    /// <returns>Returns an object of type <typeparamref name="T"/></returns>
    public static unsafe T ToObject(byte[] bytes, int index = 0)
    {
        fixed (byte* ff = bytes)
        {
            return *(T*)(ff + index);
        }
    }
}
