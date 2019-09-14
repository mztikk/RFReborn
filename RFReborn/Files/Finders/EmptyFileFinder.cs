using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders
{
    /// <summary>
    /// Provides methods to find empty files
    /// </summary>
    public class EmptyFileFinder : IFileFinder
    {
        /// <summary>
        /// Walks the <paramref name="root"/> path and finds all empty files.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns><see cref="IEnumerable{T}"/> of empty <see cref="FileInfo"/></returns>
        public IEnumerable<FileInfo> Find(string root)
        {
            HashSet<FileInfo> emptyFiles = new HashSet<FileInfo>();

            FileUtils.Walk(root, (FileInfo fi) =>
            {
                try
                {
                    if (fi.Length == 0)
                    {
                        emptyFiles.Add(fi);
                    }
                }
                catch (IOException) { }
            });

            return emptyFiles;
        }
    }
}
