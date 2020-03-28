using System;
using System.Collections.Generic;
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
                        if (module.Skip(dir))
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
                        if (module.Skip(file))
                        {
                            return false;
                        }
                    }

                    onFile(file);
                    return true;
                }
                );
    }
}
