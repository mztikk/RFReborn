using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RFReborn.Hashing;

namespace RFReborn.Files
{
    public class DuplicateFinder
    {
        public delegate string HASH(FileInfo fileInfo);

        public DuplicateFinder(HASH hashFunc) => HashFunc = hashFunc;

        public DuplicateFinder() : this(XXHASH) { }

        public HASH HashFunc { get; set; }

        public Dictionary<string, List<FileInfo>> Find(string root) => Find(new string[] { root });

        public Dictionary<string, List<FileInfo>> Find(string[] rootPaths)
        {
            Dictionary<string, FileInfo> cache = new Dictionary<string, FileInfo>();
            Dictionary<string, List<FileInfo>> dupes = new Dictionary<string, List<FileInfo>>();

            foreach (string root in rootPaths)
            {
                FileUtils.Walk(root, (FileInfo fi) =>
                {
                    try
                    {
                        string hash = HashFunc(fi);
                        if (cache.ContainsKey(hash))
                        {
                            if (dupes.ContainsKey(hash))
                            {
                                dupes[hash].Add(fi);
                            }
                            else
                            {
                                dupes.TryAdd(hash, new List<FileInfo> { cache[hash], fi });
                            }
                        }
                        else
                        {
                            cache.TryAdd(hash, fi);
                        }
                    }
                    catch (IOException) { }
                    catch (UnauthorizedAccessException) { }
                });
            }

            return dupes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string SHA512HASH(FileInfo input) => HashFactory.Hash("SHA512", input);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe string XXHASH(FileInfo input)
        {
            using (FileStream fRead = input.OpenRead())
            {
                byte[] buffer = new byte[fRead.Length];
                fRead.Read(buffer);
                return xxHash.Hash(buffer).ToString();
            }
        }
    }
}
