using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders;

/// <summary>
/// Defines methods to find files
/// </summary>
public interface IFileFinder
{
    /// <summary>
    /// Finds files in the <paramref name="root"/> path.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns><see cref="IEnumerable{T}"/> of <see cref="FileInfo"/></returns>
    IEnumerable<FileInfo> Find(string root);
}
