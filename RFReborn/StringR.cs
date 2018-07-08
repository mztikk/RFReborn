using System;
using System.Collections.Generic;
using System.Text;

namespace RFReborn
{
	/// <summary>
	/// Provides functionality to manipulate and operate on <see cref="string"/>
	/// </summary>
	public static class StringR
	{
		private static readonly HashSet<char> _WhitespaceChars = new HashSet<char>
		{
			'\u0020',
			'\u00A0',
			'\u1680',
			'\u2000',
			'\u2001',
			'\u2002',
			'\u2003',
			'\u2004',
			'\u2005',
			'\u2006',
			'\u2007',
			'\u2008',
			'\u2009',
			'\u200A',
			'\u202F',
			'\u205F',
			'\u3000',
			'\u2028',
			'\u2029',
			'\u0009',
			'\u000A',
			'\u000B',
			'\u000C',
			'\u000D',
			'\u0085'
		};

		/// <summary>
		/// Removes all whitespace chars from a string and returns it.
		/// </summary>
		/// <param name="input">String to remove whitespace from</param>
		/// <returns>A new string without whitespace</returns>
		public static string RemoveWhitespace(string input) => RemoveChars(input, _WhitespaceChars);

		/// <summary>
		/// Removes the specified chars from the string and returns it.
		/// </summary>
		/// <param name="input">String where the chars should be removed from</param>
		/// <param name="chars">The chars to remove</param>
		/// <returns>A new string, without the chars</returns>
		public static string RemoveChars(string input, ICollection<char> chars)
		{
			var len = input.Length;
			var src = input.ToCharArray();
			var dstIdx = 0;
			for (var i = 0; i < len; i++)
			{
				var ch = src[i];
				if (chars.Contains(ch))
				{
					continue;
				}

				src[dstIdx++] = ch;
			}

			return new string(src, 0, dstIdx);
		}
	}
}
