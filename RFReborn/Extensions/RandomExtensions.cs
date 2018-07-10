using System.Collections.Generic;

namespace RFReborn.Extensions
{
	/// <summary>
	/// Extends <see cref="System.Random"/>
	/// </summary>
	public static class RandomExtensions
	{
		/// <summary>
		/// Chooses a random item inside of the provided <see cref="IList{T}"/> and returns it.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="random">Random instance</param>
		/// <param name="list">List to choose random item from</param>
		/// <returns>Random item inside of list, default in case of a failure.</returns>
		public static T Choice<T>(this System.Random random, IList<T> list)
		{
			if (list.Count <= 0)
			{
				return default;
			}

			return list[random.Next(0, list.Count)];
		}

		/// <summary>
		/// Chooses a random item inside of the provided <see cref="ICollection{T}"/> and returns it.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="random">Random instance</param>
		/// <param name="collection">Collection to choose random item from</param>
		/// <returns>Random item inside of collection, default in case of a failure.></returns>
		public static T Choice<T>(this System.Random random, ICollection<T> collection)
		{
			var rng = random.Next(0, collection.Count);
			var i = 0;
			foreach (var item in collection)
			{
				if (i == rng)
				{
					return item;
				}

				i++;
			}

			return default;
		}

		/// <summary>
		/// Chooses a random item inside of the provided <see cref="IEnumerable{T}"/> and returns it.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="random">Random instance</param>
		/// <param name="enumerable">Enumerable to choose random item from</param>
		/// <returns>Random item inside of Enumerable, default in case of a failure.></returns>
		public static T Choice<T>(this System.Random random, IEnumerable<T> enumerable)
		{
			var size = 0;
			var i = 0;
			foreach (var item in enumerable)
			{
				size++;
			}
			var rng = random.Next(0, size);
			foreach (var item in enumerable)
			{
				if (i == size)
				{
					return item;
				}

				i++;
			}

			return default;
		}
	}
}