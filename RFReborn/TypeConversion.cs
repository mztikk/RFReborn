using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace RFReborn
{
	internal unsafe static class TypeConversion<T> where T : unmanaged
	{
		public static Type Type { get; }

		public static TypeCode TypeCode { get; }

		public static int Size { get; }

		static TypeConversion()
		{
			Type = typeof(T);
			TypeCode = Type.GetTypeCode(Type);
			Size = TypeCode == TypeCode.Boolean ? 1 : sizeof(T);
		}

		public static byte[] ToByteArray(T obj)
		{
			switch (TypeCode)
			{
				case TypeCode.Boolean:
					return BitConverter.GetBytes((bool)(object)obj);
				case TypeCode.Char:
					return Encoding.UTF8.GetBytes(new[] { (char)(object)obj });
				case TypeCode.SByte:
				case TypeCode.Byte:
					return new byte[] { (byte)(object)obj };
				case TypeCode.Int16:
					return BitConverter.GetBytes((short)(object)obj);
				case TypeCode.UInt16:
					return BitConverter.GetBytes((ushort)(object)obj);
				case TypeCode.Int32:
					return BitConverter.GetBytes((int)(object)obj);
				case TypeCode.UInt32:
					return BitConverter.GetBytes((uint)(object)obj);
				case TypeCode.Int64:
					return BitConverter.GetBytes((long)(object)obj);
				case TypeCode.UInt64:
					return BitConverter.GetBytes((ulong)(object)obj);
				case TypeCode.Single:
					return BitConverter.GetBytes((float)(object)obj);
				case TypeCode.Double:
					return BitConverter.GetBytes((double)(object)obj);
				case TypeCode.Decimal:
					var bits = decimal.GetBits((decimal)(object)obj);
					var ba = new BitArray(bits);
					var bytes = new byte[ba.Length / 8];
					ba.CopyTo(bytes, 0);
					return bytes;
			}

			var structBytes = new byte[Size];
			var ptr = Marshal.AllocHGlobal(Size);
			Marshal.StructureToPtr(obj, ptr, false);
			Marshal.Copy(ptr, structBytes, 0, Size);
			Marshal.FreeHGlobal(ptr);
			return structBytes;
		}

		public static T ToObject(byte[] btArray, int index = 0)
		{
			switch (TypeCode)
			{
				case TypeCode.Boolean:
					return (T)(object)BitConverter.ToBoolean(btArray, index);
				case TypeCode.Char:
					return (T)(object)Encoding.UTF8.GetChars(btArray)[index];
				case TypeCode.SByte:
				case TypeCode.Byte:
					return (T)(object)btArray[index];
				case TypeCode.Int16:
					return (T)(object)BitConverter.ToInt16(btArray, index);
				case TypeCode.UInt16:
					return (T)(object)BitConverter.ToUInt16(btArray, index);
				case TypeCode.Int32:
					return (T)(object)BitConverter.ToInt32(btArray, index);
				case TypeCode.UInt32:
					return (T)(object)BitConverter.ToUInt32(btArray, index);
				case TypeCode.Int64:
					return (T)(object)BitConverter.ToInt64(btArray, index);
				case TypeCode.UInt64:
					return (T)(object)BitConverter.ToUInt64(btArray, index);
				case TypeCode.Single:
					return (T)(object)BitConverter.ToSingle(btArray, index);
				case TypeCode.Double:
					return (T)(object)BitConverter.ToDouble(btArray, index);
				case TypeCode.Decimal:
					return (T)(object)new decimal(
						BitConverter.ToInt32(btArray, index),
						BitConverter.ToInt32(btArray, index + 4),
						BitConverter.ToInt32(btArray, index + 8),
						btArray[index + 15] == 128,
						btArray[index + 14]);
			}

			var ptr = Marshal.AllocHGlobal(Size);
			Marshal.Copy(btArray, index, ptr, Size);
			var rtn = Marshal.PtrToStructure<T>(ptr);
			Marshal.FreeHGlobal(ptr);
			return rtn;
		}
	}
}