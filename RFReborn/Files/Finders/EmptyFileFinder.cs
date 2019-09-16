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
            foreach (string file in FileUtils.Walk(root, FileSystemEnumeration.FilesOnly))
            {
                FileInfo fi;
                long len;
                try
                {
                    fi = new FileInfo(file);
                    len = fi.Length;
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
                catch (IOException)
                {
                    continue;
                }

                if (len == 0)
                {
                    yield return fi;
                }
            }
        }
    }
}
