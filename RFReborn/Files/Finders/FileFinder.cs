using System.Collections.Generic;
using System.IO;

namespace RFReborn.Files.Finders
{
    public class FileFinder : IFileFinder
    {
        public FileSearchOptions? searchOptions;

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

                if (searchOptions?.Compare(file) == true)
                {
                    yield return file;
                }
            }
        }
    }
}
