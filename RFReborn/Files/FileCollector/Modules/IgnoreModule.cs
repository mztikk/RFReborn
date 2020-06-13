using System;
using System.Collections.Generic;

namespace RFReborn.Files.FileCollector.Modules
{
    /// <summary>
    /// <see cref="ICollectingModule"/> implementation for ignore wildcard patterns
    /// </summary>
    [Obsolete("IgnoreModule is deprecated, use ListModule instead.")]
    public class IgnoreModule : ICollectingModule
    {
        private readonly IEnumerable<string> _ignorePatterns;

        /// <summary>
        /// Constructs a new <see cref="IgnoreModule"/> with given patterns
        /// </summary>
        /// <param name="ignorePatterns">patterns to decide if a path should be skipped or taken</param>
        public IgnoreModule(IEnumerable<string> ignorePatterns) => _ignorePatterns = ignorePatterns;

        /// <inheritdoc />
        public bool Skip(string path) => WildcardHelper.IsMatch(path, _ignorePatterns);

        /// <inheritdoc />
        public bool Take(string path) => !WildcardHelper.IsMatch(path, _ignorePatterns);
    }

    /// <summary>
    /// Extensions class for IgnoreModule and FileCollector
    /// </summary>
    public static class IgnoreModuleExtensions
    {
        /// <summary>
        /// Adds a new <see cref="IgnoreModule"/> to a <see cref="FileCollector"/> with given patterns
        /// </summary>
        /// <param name="fileCollector">FileCollector to add module to</param>
        /// <param name="ignorePatterns">patterns to use</param>
        [Obsolete("IgnoreModule is deprecated, use ListModule instead.")]
        public static FileCollector AddIgnoreModule(this FileCollector fileCollector, IEnumerable<string> ignorePatterns)
        {
            fileCollector.AddModule(new IgnoreModule(ignorePatterns));
            return fileCollector;
        }
    }
}
