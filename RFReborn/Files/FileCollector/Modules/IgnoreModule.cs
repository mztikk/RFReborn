using System.Collections.Generic;

namespace RFReborn.Files.FileCollector.Modules
{
    /// <summary>
    /// <see cref="ICollectingModule"/> implementation for ignore wildcard patterns
    /// </summary>
    public class IgnoreModule : ICollectingModule
    {
        private readonly IEnumerable<string> _ignorePatterns;

        /// <summary>
        /// Constructs a new <see cref="IgnoreModule"/> with given patterns
        /// </summary>
        /// <param name="ignorePatterns">patterns to decide if a path should be skipped or taken</param>
        public IgnoreModule(IEnumerable<string> ignorePatterns) => _ignorePatterns = ignorePatterns;

        /// <inheritdoc />
        public bool Skip(string path) => IgnoreHelper.IsIgnored(path, _ignorePatterns);

        /// <inheritdoc />
        public bool Take(string path) => !IgnoreHelper.IsIgnored(path, _ignorePatterns);
    }
}
