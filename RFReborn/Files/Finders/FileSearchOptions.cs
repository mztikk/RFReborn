using System;
using System.IO;
using RFReborn.Comparison;

namespace RFReborn.Files.Finders
{
    /// <summary>
    /// Dynamic Comparer for <see cref="FileInfo"/>
    /// </summary>
    public class FileSearchOptions : DynamicComparer<FileInfo>
    {
        /// <summary>
        /// Constructs a new <see cref="FileSearchOptions"/> with default value of <see cref="DynamicComparisonType.NonNull"/>
        /// </summary>
        public FileSearchOptions() : base(DynamicComparisonType.NonNull) { }

        /// <summary>
        /// string representing the extension part of the file
        /// </summary>
        public string? Extension { get; set; }

        /// <summary>
        /// full path of the directory or file
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// value that determines if the current file is read only
        /// </summary>
        public bool? IsReadOnly { get; set; }

        /// <summary>
        /// time the current file or directory was last accessed
        /// </summary>
        public DateTime? LastAccessTime { get; set; }

        /// <summary>
        /// time, in coordinated universal time (UTC), that the current file or directory was last accessed
        /// </summary>
        public DateTime? LastAccessTimeUtc { get; set; }

        /// <summary>
        /// time when the current file or directory was last written to
        /// </summary>
        public DateTime? LastWriteTime { get; set; }

        /// <summary>
        /// time, in coordinated universal time (UTC), when the current file or directory was last written to
        /// </summary>
        public DateTime? LastWriteTimeUtc { get; set; }

        /// <summary>
        /// size, in bytes, of the current file
        /// </summary>
        public long? Length { get; set; }

        /// <summary>
        /// name of the file
        /// </summary>
        public string? Name { get; set; }
    }
}
