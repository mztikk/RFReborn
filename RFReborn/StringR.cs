using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using RFReborn.Extensions;

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
        /// All valid Hexadecimal characters
        /// </summary>
        public const string Hex = Numbers + "ABCDEFabcdef";

        /// <summary>
        /// All valid Hexadecimal characters as a key based Lookup Dictionary
        /// </summary>
        public static readonly IReadOnlyDictionary<char, bool> s_hexLookup = new ReadOnlyDictionary<char, bool>(Hex.ToLookup());

        /// <summary>
        /// Gets the escaped unicode value string represantion of a char
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetEscapeSequence(char c) => "\\u" + ((int)c).ToString("X4");

        private static readonly HashSet<char> s_whitespaceChars = new HashSet<char>
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
        public static string RemoveWhitespace(string input) => RemoveChars(input, s_whitespaceChars);

        /// <summary>
        /// Removes the specified chars from the string and returns it.
        /// </summary>
        /// <param name="input">String where the chars should be removed from.</param>
        /// <param name="chars">The chars to remove.</param>
        /// <returns>A new string, without the chars.</returns>
        public static string RemoveChars(string input, ICollection<char> chars)
        {
            int len = input.Length;
            char* rtn = stackalloc char[len];
            int dstIdx = 0;
            for (int i = 0; i < len; i++)
            {
                char ch = input[i];
                if (chars.Contains(ch))
                {
                    continue;
                }

                rtn[dstIdx++] = ch;
            }

            return new string(rtn, 0, dstIdx);
        }

        /// <summary>
        /// Converts a byte array to its hex string representation.
        /// </summary>
        /// <param name="input">Bytes to convert.</param>
        /// <returns>Hex string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ByteArrayToHexString(byte[] input) => BitConverter.ToString(input).Replace("-", "");

        /// <summary>
        /// Splits a <see cref="string"/> in parts of <paramref name="n"/> length.
        /// </summary>
        /// <param name="str">String to split.</param>
        /// <param name="n">Length of split parts.</param>
        /// <returns>Array of strings that each contain a split part.</returns>
        /// <exception cref="ArgumentNullException">string is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">n is smaller or equal to 0</exception>
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

            string[] rtn = new string[(int)Math.Ceiling((double)str.Length / n)];

            fixed (void* vp = str)
            {
                char* p = (char*)vp;
                int i = 0;
                int j = 0;
                while (i < str.Length)
                {
                    int trueLen = Math.Min(n, str.Length - i);

                    rtn[j++] = new string(p, i, trueLen);
                    i += trueLen;
                }
            }

            return rtn;
        }

        /// <summary>
        /// Splits the string every nth char with the specified separator./Inserts the separator every nth char.
        /// </summary>
        /// <param name="str">String to split.</param>
        /// <param name="n">Length of split parts.</param>
        /// <param name="seperator">Separator to use between the split parts.</param>
        /// <returns>A new string with the separator every nth char.</returns>
        public static string InSplit(string str, int n, string seperator)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str) + " can't be null.");
            }

            if (n <= 0 || string.IsNullOrEmpty(seperator))
            {
                return str;
            }

            int extraSize = (int)Math.Ceiling((double)str.Length / n) * seperator.Length;
            extraSize -= seperator.Length;
            int sSize = sizeof(char) * seperator.Length;
            int newSize = str.Length + extraSize;
            char[] rtn = new char[newSize];
            int i = 0;
            fixed (void* vp = rtn)
            {
                char* rp = (char*)vp;
                fixed (void* vs = seperator)
                {
                    while (i < str.Length)
                    {
                        *rp = str[i++];
                        rp++;
                        if (i % n == 0)
                        {
                            Buffer.MemoryCopy(vs, rp, sSize, sSize);
                            rp += seperator.Length;
                        }
                    }
                }
            }

            return new string(rtn);
        }

        /// <summary>
        /// Returns a new string where the first char is capitalized.
        /// </summary>
        /// <param name="s">String to capitalize</param>
        /// <returns>A new string with the first char capitalized</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CapitalizeFirstLetter(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length == 1)
            {
                return s.ToUpperInvariant();
            }

            string rtn = new string(s);
            rtn.CapitalizeFirstLetterSelf();

            return rtn;
        }

        /// <summary>
        /// Capitalizes the first char in a string
        /// </summary>
        /// <param name="s">String to capitalize</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CapitalizeFirstLetterSelf(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            fixed (void* sP = s)
            {
                char* sPC = (char*)sP;
                *sPC = char.ToUpperInvariant(*sPC);
            }
        }

        /// <summary>
        /// Checks if a given pattern matches a string
        /// </summary>
        /// <param name="input">input to check</param>
        /// <param name="pattern">pattern to match</param>
        /// <param name="wildcard">matches any sequence of characters</param>
        /// <param name="singlewildcard">matches any single character</param>
        /// <returns>returns <see langword="true"/> if pattern matches; <see langword="false"/> otherwise</returns>
        public static bool WildcardMatch(string input, string pattern, char wildcard = '*', char singlewildcard = '?')
        {
            for (int i = 0, j = 0; i < input.Length; i++)
            {
                if (j == pattern.Length)
                {
                    return false;
                }

                char inputChar = input[i];
                char patternChar = pattern[j];
                if (patternChar == wildcard)
                {
                    if (j + 1 < pattern.Length && inputChar == pattern[j + 1])
                    {
                        j += 2;
                    }

                    continue;
                }
                if (patternChar == singlewildcard)
                {
                    j++;
                    continue;
                }

                if (inputChar == patternChar)
                {
                    j++;
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
