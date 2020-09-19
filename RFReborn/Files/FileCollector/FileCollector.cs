using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RFReborn.Files.FileCollector.Modules;

namespace RFReborn.Files.FileCollector
{
    /// <summary>
    /// Modular class that collects files
    /// </summary>
    public class FileCollector
    {
        private readonly ICollection<ICollectingModule> _modules = new List<ICollectingModule>();

        /// <summary>
        /// Adds a new module
        /// </summary>
        /// <param name="module">module to add</param>
        public FileCollector AddModule(ICollectingModule module)
        {
            _modules.Add(module);
            return this;
        }

        /// <summary>
        /// Gets all files based on <see cref="ICollectingModule"/>s
        /// </summary>
        /// <param name="root">Root path to travel</param>
        /// <param name="onFile">Action to execute on every file</param>
        public void GetFiles(string root, Action<string> onFile) => FileUtils.Walk(
                root,
                (string dir) =>
                {
                    foreach (ICollectingModule module in _modules)
                    {
                        if (module.Skip(dir) || !module.Take(dir))
                        {
                            return false;
                        }
                    }

                    return true;
                },
                (string file) =>
                {
                    foreach (ICollectingModule module in _modules)
                    {
                        if (module.Skip(file) || !module.Take(file))
                        {
                            return false;
                        }
                    }

                    onFile(file);
                    return true;
                }
                );

        /// <summary>
        /// Enumerates all files based on <see cref="ICollectingModule"/>s
        /// </summary>
        /// <param name="root">Root path to traverse</param>
        public IEnumerable<string> EnumerateFiles(string root)
        {
            foreach (string file in FileUtils.GetFiles(root, SkipDirectory))
            {
                bool takeFile = true;

                foreach (ICollectingModule module in _modules)
                {
                    if (module.Skip(file) || !module.Take(file))
                    {
                        takeFile = false;
                        break;
                    }
                }

                if (takeFile)
                {
                    yield return file;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SkipDirectory(DirectoryInfo dir) => SkipDirectory(FileUtils.NormalizePath(dir.FullName));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SkipDirectory(string path)
        {
            foreach (ICollectingModule module in _modules)
            {
                if (module.Skip(path) || !module.Take(path))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
