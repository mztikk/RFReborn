using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RFReborn.Hashing;

namespace RFReborn.Files
{
    /// <summary>
    /// Provides methods to find duplicate files by storing their hashes
    /// </summary>
    public class DuplicateFinder
    {
        /// <summary>
        /// Constructs a new <see cref="DuplicateFinder"/> with a specified hash function
        /// </summary>
        /// <param name="hashFunc">Hash to use</param>
        public DuplicateFinder(Func<FileInfo, string> hashFunc) => HashFunc = hashFunc;

        /// <summary>
        /// Constructs a new <see cref="DuplicateFinder"/> with a default hash function of xxHash
        /// </summary>
        public DuplicateFinder() : this(XXHASH) { }

        /// <summary>
        /// Hash function to use
        /// </summary>
        public Func<FileInfo, string> HashFunc { get; set; }

        /// <summary>
        /// Finds all duplicates in given root path
        /// </summary>
        /// <param name="root">root path to walk</param>
        public Dictionary<string, List<FileInfo>> Find(string root) => Find(new string[] { root });

        /// <summary>
        /// Finds all duplicates in all given root paths
        /// </summary>
        /// <param name="rootPaths">root paths to walk</param>
        public Dictionary<string, List<FileInfo>> Find(string[] rootPaths)
        {
            Dictionary<string, FileInfo> cache = new Dictionary<string, FileInfo>();
            Dictionary<string, List<FileInfo>> dupes = new Dictionary<string, List<FileInfo>>();

            foreach (string root in rootPaths)
            {
                FileUtils.Walk(root, fi =>
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
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (IOException) { }
                    catch (UnauthorizedAccessException) { }
#pragma warning restore CA1031 // Do not catch general exception types
                });
            }

            return dupes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0051 // Remove unused private members
        private static string SHA512HASH(FileInfo input) => HashFactory.Hash("SHA512", input);
#pragma warning restore IDE0051 // Remove unused private members

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe string XXHASH(FileInfo input)
        {
            using FileStream fRead = input.OpenRead();
            byte[] buffer = new byte[fRead.Length];
            fRead.Read(buffer);
            return xxHash.Hash(buffer).ToString();
        }
    }
}
