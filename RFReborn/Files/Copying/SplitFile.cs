using System;

namespace RFReborn.Files.Copying;

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
        long len = file.Length;
        double t = len / (double)splitSize;
        int n = (int)Math.Ceiling(t);

        var rtn = new SplitInfo[n];
        int i = 0;
        while (len > 0)
        {
            rtn[i++] = new SplitInfo(file.Length - len, (int)Math.Min(len, splitSize));

            len -= splitSize;
        }

        return rtn;
    }
}
