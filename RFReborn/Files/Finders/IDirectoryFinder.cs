using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders
{
    /// <summary>
    /// Defines methods to find directories
    /// </summary>
    public interface IDirectoryFinder
    {
        /// <summary>
        /// Finds directories in the <paramref name="root"/> path.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="DirectoryInfo"/></returns>
        IEnumerable<DirectoryInfo> Find(string root);
    }
}
