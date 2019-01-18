using System;
using System.Collections.Generic;

namespace RFReborn
{
    /// <summary>
    /// Provides functionality to manipulate and operate on <see cref="string"/>.
    /// </summary>
    public static unsafe class StringR
    {
        /// <summary>
        /// Full latin alphabet in uppercase
        /// </summary>
        public const string AlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Full latin alphabet in lowercase
        /// </summary>
        public const string AlphabetLower = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Full latin alphabet in uppercase plus all numbers from 0 to 9
        /// </summary>
        public const string Alphanumeric = AlphabetUpper + Numbers;

        /// <summary>
        /// Full latin alphabet in uppercase, all numbers from 0 to 9 and all special characters (except space)
        /// </summary>
        public const string Chars = AlphabetUpper + Numbers + Special;

        /// <summary>
        /// Full latin alphabet in uppercase, all numbers from 0 to 9, all special characters (including space)
        /// </summary>
        public const string CharsWhite = Chars + " ";

        /// <summary>
        /// Consonants of the latin alphabet
        /// </summary>
        public const string Consonants = "BCDFGHJKLMNPQRSTVWXYZ";

        /// <summary>
        /// Vowels of the latin alphabet
        /// </summary>
        public const string Vowels = "AEIOU";

        /// <summary>
        /// Numbers from 0 to 9
        /// </summary>
        public const string Numbers = "0123456789";

        /// <summary>
        /// Special characters
        /// </summary>
        public const string Special = ".:-_,;#+*?=)(/&%$§!\"\\<>|@'´`~}][{";

        /// <summary>
        /// Gets the escaped unicode value string represantion of a char
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetEscapeSequence(char c)
        {
            return "\\u" + ((int)c).ToString("X4");
        }

        private static readonly HashSet<char> WhitespaceChars = new HashSet<char>
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
            '\u0085',
        };

        /// <summary>
        /// Removes all whitespace chars from a string and returns it.
        /// </summary>
        /// <param name="input">String to remove whitespace from.</param>
        /// <returns>A new string without whitespace.</returns>
        public static string RemoveWhitespace(string input) => RemoveChars(input, WhitespaceChars);

        /// <summary>
        /// Removes the specified chars from the string and returns it.
        /// </summary>
        /// <param name="input">String where the chars should be removed from.</param>
        /// <param name="chars">The chars to remove.</param>
        /// <returns>A new string, without the chars.</returns>
        public static string RemoveChars(string input, ICollection<char> chars)
        {
            var len = input.Length;
            var rtn = stackalloc char[len];
            var dstIdx = 0;
            for (var i = 0; i < len; i++)
            {
                var ch = input[i];
                if (chars.Contains(ch))
                {
                    continue;
                }

                rtn[dstIdx++] = ch;
            }

            return new string(rtn, 0, dstIdx);
        }

        // https://stackoverflow.com/a/24343727
        private static readonly uint[] Lookup32Unsafe = CreateLookup32Unsafe();

        private static uint[] CreateLookup32Unsafe()
        {
            var result = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("X2");
                if (BitConverter.IsLittleEndian)
                {
                    result[i] = s[0] + ((uint)s[1] << 16);
                }
                else
                {
                    result[i] = s[1] + ((uint)s[0] << 16);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a byte array to its hex string representation.
        /// </summary>
        /// <param name="input">Bytes to convert.</param>
        /// <returns>Hex string.</returns>
        public static string ByteArrayToHexString(byte[] input)
        {
            var result = stackalloc char[input.Length * 2];
            var resultP2 = (uint*)result;
            fixed (uint* lookupP = Lookup32Unsafe)
            {
                fixed (byte* bytesP = input)
                {
                    for (var i = 0; i < input.Length; i++)
                    {
                        resultP2[i] = lookupP[bytesP[i]];
                    }
                }
            }

            return new string(result);
        }

        /// <summary>
        /// Splits a <see cref="string"/> in parts of <paramref name="n"/> length.
        /// </summary>
        /// <param name="str">String to split.</param>
        /// <param name="n">Length of split parts.</param>
        /// <returns>Array of strings that each contain a split part.</returns>
        public static string[] SplitN(string str, int n)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str) + " can't be null.");
            }

            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n) + " must a positive integer and greater than zero.");
            }

            var rtn = new string[(int)Math.Ceiling((double)str.Length / n)];

            fixed (void* vp = str)
            {
                var p = (char*)vp;
                var i = 0;
                var j = 0;
                while (i < str.Length)
                {
                    var trueLen = Math.Min(n, str.Length - i);

                    rtn[j++] = new string(p, i, trueLen);
                    i += trueLen;
                }
            }

            return rtn;
        }
    }
}
