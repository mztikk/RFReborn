using System;
using System.IO;

namespace RFReborn.Files.Copying
{
    /// <summary>
    /// Class that wraps a <see cref="FileInfo"/> and splits it into segments, each segment being the size of <see cref="SplitSize"/>
    /// </summary>
    internal class SplitFile
    {
        public SplitFile(FileInfo file, long splitSize)
        {
            File = file;
            SplitSize = splitSize;
            SplitInfos = GetSplitInfos(file, splitSize);
        }

        public FileInfo File { get; }
        public long SplitSize { get; }
        public SplitInfo[] SplitInfos { get; }

        private static SplitInfo[] GetSplitInfos(FileInfo file, long splitSize)
        {
            long l = file.Length;
            double t = l / (double)splitSize;
            int n = (int)Math.Ceiling((double)t);

            var rtn = new SplitInfo[n];
            int i = 0;
            while (l > 0)
            {
                rtn[i++] = new SplitInfo(file.Length - l, (int)Math.Min(l, splitSize));

                l -= splitSize;
            }

            return rtn;
        }
    }
}
