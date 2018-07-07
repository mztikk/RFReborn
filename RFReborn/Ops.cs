using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RFReborn
{
	/// <summary>
	/// Provides general operations
	/// </summary>
	public static class Ops
	{
		/// <summary>
		/// Swaps two variables.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="m1">First value</param>
		/// <param name="m2">Second value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(ref T m1, ref T m2)
		{
			var tmp = m1;
			m1 = m2;
			m2 = tmp;
		}

		/// <summary>
		/// Merges all given <see cref="IList{T}"/> into one and returns it.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="listArray">All <see cref="IList{T}"/> to merge</param>
		/// <returns>The merged <see cref="IList{T}"/></returns>
		public static IList<T> Merge<T>(params IList<T>[] listArray) => Merge(enumList: listArray);

		/// <summary>
		/// Merges all given <see cref="IList{T}"/> into one and returns it.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="enumList"><see cref="IEnumerable{T}"/> of all <see cref="IList{T}"/> to merge</param>
		/// <returns>The merged <see cref="IList{T}"/></returns>
		public static IList<T> Merge<T>(IEnumerable<IList<T>> enumList)
		{
			var newSize = 0;
			foreach (var list in enumList)
			{
				newSize += list.Count;
			}

			var rtn = new T[newSize];
			var lastIndex = 0;
			foreach (var list in enumList)
			{
				list.CopyTo(rtn, lastIndex);
				lastIndex += list.Count;
			}

			return rtn;
		}
	}
}