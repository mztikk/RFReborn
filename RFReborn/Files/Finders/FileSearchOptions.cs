using System;
using System.IO;
using RFReborn.Comparison;

namespace RFReborn.Files.Finders
{
    public class FileSearchOptions : DynamicComparer<FileInfo>
    {
        public FileSearchOptions() => comparisonType = DynamicComparisonType.NonNull;

        public string Extension { get; set; }

        public string FullName { get; set; }

        public bool? IsReadOnly { get; set; }

        public DateTime? LastAccessTime { get; set; }

        public DateTime? LastAccessTimeUtc { get; set; }

        public DateTime? LastWriteTime { get; set; }

        public DateTime? LastWriteTimeUtc { get; set; }

        public long? Length { get; set; }

        public string Name { get; set; }
    }
}
