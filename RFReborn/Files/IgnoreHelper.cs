using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files
{
    /// <summary>
    /// Class that provides methods to ignore paths based on wildcard matching patterns
    /// </summary>
    public static class IgnoreHelper
    {
        /// <summary>
        /// Check if the path is ignored by the given pattern
        /// </summary>
        /// <param name="path">path to check</param>
        /// <param name="pattern">pattern to use</param>
        public static bool IsIgnored(string path, string pattern)
        {
            if (StringR.WildcardMatch(path, pattern))
            {
                return true;
            }

            DirectoryInfo di = new DirectoryInfo(path);
            return StringR.WildcardMatch(di.Name, pattern);
        }

        /// <summary>
        /// Check if the path is ignored by any of the given patterns
        /// </summary>
        /// <param name="path">path to check</param>
        /// <param name="patterns">patterns to use</param>
        public static bool IsIgnored(string path, IEnumerable<string> patterns)
        {
            foreach (string pattern in patterns)
            {
                if (IsIgnored(path, pattern))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reads all ignore patterns from a file and yields them as an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="file">file to read</param>
        /// <remarks>
        /// Lines starting with "#" are ignored and can be used as comments
        /// <para/>
        /// Empty lines are skipped
        /// </remarks>
        public static IEnumerable<string> GetIgnorePatternsFromFile(string file)
        {
            foreach (string line in File.ReadAllLines(file))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith('#'))
                {
                    continue;
                }

                yield return line;
            }
        }
    }
}
