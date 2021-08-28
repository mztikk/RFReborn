using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders
{
    /// <summary>
    /// Provides methods to find files by comparing <see cref="FileInfo"/>
    /// </summary>
    public class FileFinder : IFileFinder
    {
        /// <summary>
        /// Search options to use when comparing files
        /// </summary>
        public FileSearchOptions? SearchOptions;

        /// <summary>
        /// Walks the <paramref name="root"/> path and finds all files matching our <see cref="SearchOptions"/>.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns><see cref="IEnumerable{T}"/> of empty <see cref="FileInfo"/></returns>
        public IEnumerable<FileInfo> Find(string root)
        {
            foreach (string filePath in FileUtils.Walk(root, FileSystemEnumeration.FilesOnly))
            {
                FileInfo file;
                try
                {
                    file = new FileInfo(filePath);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
                catch (IOException)
                {
                    continue;
                }

                if (SearchOptions?.Compare(file) == true)
                {
                    yield return file;
                }
            }
        }
    }
}
