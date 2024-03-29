﻿namespace RFReborn;

/// <summary>
/// Provides functionality to manipulate and operate on <see cref="string"/>.
/// </summary>
public static class StringR
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
    /// Gets the escaped unicode value string representation of a char
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string GetEscapeSequence(char c) => "\\u" + ((int)c).ToString("X4");

    private static readonly HashSet<char> s_whitespaceChars = new()
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
    public static unsafe string RemoveChars(string input, ICollection<char> chars)
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
    public static unsafe string[] SplitN(string str, int n)
    {
        if (str is null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be a positive integer and greater than zero.");
        }

        string[] rtn = new string[(long)Math.Ceiling((double)str.Length / n)];

        fixed (char* p = str)
        {
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
    /// <param name="separator">Separator to use between the split parts.</param>
    /// <returns>A new string with the separator every nth char.</returns>
    public static unsafe string InSplit(string str, int n, string separator)
    {
        if (str is null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (n <= 0 || string.IsNullOrEmpty(separator))
        {
            return str;
        }

        int extraSize = (int)Math.Ceiling((double)str.Length / n) * separator.Length;
        extraSize -= separator.Length;
        int sSize = sizeof(char) * separator.Length;
        int newSize = str.Length + extraSize;
        char[] rtn = new char[newSize];
        int i = 0;
        fixed (void* vp = rtn)
        {
            char* rp = (char*)vp;
            fixed (void* vs = separator)
            {
                while (i < str.Length)
                {
                    *rp = str[i++];
                    rp++;
                    if (i % n == 0 && i < str.Length)
                    {
                        Buffer.MemoryCopy(vs, rp, sSize, sSize);
                        rp += separator.Length;
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

        string rtn = new(s);
        rtn.CapitalizeFirstLetterSelf();

        return rtn;
    }

    /// <summary>
    /// Capitalizes the first char in a string
    /// </summary>
    /// <param name="s">String to capitalize</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void CapitalizeFirstLetterSelf(this string s)
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
    /// <param name="ignoreCasing">Case insensitive comparison</param>
    /// <returns>returns <see langword="true"/> if pattern matches; <see langword="false"/> otherwise</returns>
    public static bool WildcardMatch(string input, string pattern, char wildcard, char singlewildcard, bool ignoreCasing)
    {
        bool[,] lookup = new bool[input.Length + 1, pattern.Length + 1];

        lookup[0, 0] = true;

        for (int j = 1; j <= pattern.Length; j++)
        {
            if (pattern[j - 1] == wildcard)
            {
                lookup[0, j] = lookup[0, j - 1];
            }
        }

        for (int i = 1; i <= input.Length; i++)
        {
            for (int j = 1; j <= pattern.Length; j++)
            {
                if (pattern[j - 1] == wildcard)
                {
                    lookup[i, j] = lookup[i, j - 1] || lookup[i - 1, j];
                }
                else if (pattern[j - 1] == singlewildcard)
                {
                    lookup[i, j] = lookup[i - 1, j - 1];
                }
                else
                {
                    if (ignoreCasing)
                    {
                        if (char.ToLower(input[i - 1]) == char.ToLower(pattern[j - 1]))
                        {
                            lookup[i, j] = lookup[i - 1, j - 1];
                        }
                        else
                        {
                            lookup[i, j] = false;
                        }
                    }
                    else if (input[i - 1] == pattern[j - 1])
                    {
                        lookup[i, j] = lookup[i - 1, j - 1];
                    }
                    else
                    {
                        lookup[i, j] = false;
                    }
                }
            }
        }

        return lookup[input.Length, pattern.Length];
    }

    /// <summary>
    /// Checks if a given pattern matches a string, with * as a wildcard and ? as a single wildcard
    /// </summary>
    /// <param name="input">input to check</param>
    /// <param name="pattern">pattern to match</param>
    /// <param name="ignoreCasing">Case insensitive comparison</param>
    /// <returns>returns <see langword="true"/> if pattern matches; <see langword="false"/> otherwise</returns>
    public static bool WildcardMatch(string input, string pattern, bool ignoreCasing) => WildcardMatch(input, pattern, '*', '?', ignoreCasing);

    /// <summary>
    /// Checks if a given pattern matches a string, with * as a wildcard and ? as a single wildcard and case sensitive comparison
    /// </summary>
    /// <param name="input">input to check</param>
    /// <param name="pattern">pattern to match</param>
    /// <returns>returns <see langword="true"/> if pattern matches; <see langword="false"/> otherwise</returns>
    public static bool WildcardMatch(string input, string pattern) => WildcardMatch(input, pattern, '*', '?', false);

    private static readonly string[] s_byteSizeDescriptors = new string[] { "byte", "kb", "mb", "gb", "tb", "pb", "eb", "zb", "yb" };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (long, string) GetDescriptorIndexAndNumString(string input)
    {
        string[] matchers = s_byteSizeDescriptors;
        int match = 0;
        string numString = input;
        for (int i = 0; i < matchers.Length; i++)
        {
            if (input.EndsWith(matchers[i], StringComparison.OrdinalIgnoreCase))
            {
                match = i;
                numString = input.Substring(0, input.Length - matchers[i].Length);
                break;
            }
        }

        return (match, numString);
    }

    /// <summary>
    /// Turns a string containing a byte size description (e.g. 50kb, 100mb) into its byte value
    /// </summary>
    /// <param name="input">Input to convert</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null</exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="OverflowException"></exception>
    /// <returns>The converted/parsed number</returns>
    public static long StringToByteSize(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        (long match, string numString) = GetDescriptorIndexAndNumString(input);

        long num = long.Parse(numString);

        long conversion = (long)Math.Pow(1000, match);
        return num * conversion;
    }

    /// <summary>
    /// Turns a string containing a byte size description (e.g. 50kb, 100mb) into its byte value
    /// </summary>
    /// <param name="input">Input to convert</param>
    /// <param name="result"></param>
    /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; <see langword="false"/> otherwise.</returns>
    public static bool TryStringToByteSize(string? input, out long result)
    {
        if (input is null)
        {
            result = 0;
            return false;
        }

        (long match, string numString) = GetDescriptorIndexAndNumString(input);

        if (!long.TryParse(numString, out long num))
        {
            result = 0;
            return false;
        }

        long conversion = (long)Math.Pow(1000, match);
        result = num * conversion;
        return true;
    }

    /// <summary>
    /// Turns a string containing a byte size description (e.g. 50kb, 100mb) into its byte value
    /// </summary>
    /// <param name="input">Input to convert</param>
    /// <param name="result"></param>
    /// <returns><see langword="true"/> if <paramref name="input"/> was converted successfully; <see langword="false"/> otherwise.</returns>
    [Obsolete("Use TryStringToByteSize(string, out long) instead")]
    public static bool StringToByteSize(string? input, out long result) => TryStringToByteSize(input, out result);
}
