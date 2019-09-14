using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders
{
    /// <summary>
    /// Provides methods to find empty directories
    /// </summary>
    public class EmptyDirectoryFinder : IDirectoryFinder
    {
        /// <summary>
        /// <para>Walks the <paramref name="root"/> path and finds all empty directories.</para>
        /// <para>If a directory contains empty directores only the top level empty one will be returned.</para>
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns><see cref="IEnumerable{T}"/> of empty <see cref="DirectoryInfo"/></returns>
        public IEnumerable<DirectoryInfo> Find(string root)
        {
            HashSet<DirectoryInfo> emptyDirs = new HashSet<DirectoryInfo>();

            FileUtils.Walk(root, (DirectoryInfo di) =>
            {
                (ulong fileCount, ulong folderCount) = FileUtils.CountFilesAndFolders(di.FullName);
                if (fileCount == 0)
                {
                    emptyDirs.Add(di);

                    if (folderCount > 0)
                    {
                        return false;
                    }
                }

                return true;
            });

            return emptyDirs;
        }
    }
}
