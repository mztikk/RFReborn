using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RFReborn.Comparison;

namespace RFReborn.Copying
{
    /// <summary>
    /// Class that provides methods to copy files by splitting them into segments and comparing them and copying only segments that changed instead of copying everything at once.
    /// Useful for big files
    /// </summary>
    public static class SegmentCopy
    {
        private static void StreamCopy(Stream origin, Stream dest, int length, Action<long>? onWrite)
        {
            int array_length = Math.Min((int)Math.Pow(2, 19), length);
            byte[] dataArray = new byte[array_length];
            using BinaryReader bread = new BinaryReader(origin, Encoding.UTF8, true);
            using BinaryWriter bwrite = new BinaryWriter(dest, Encoding.UTF8, true);
            int read;
            int totalRead = 0;
            while ((read = bread.Read(dataArray, 0, Math.Min(array_length, length - totalRead))) > 0 && totalRead < length)
            {
                bwrite.Write(dataArray, 0, read);
                totalRead += read;
                onWrite?.Invoke(read);
            }
        }

        /// <summary>
        /// Copies a <paramref name="source"/> file to a <paramref name="destination"/> splitting it into segments each sized <paramref name="splitSize"/>
        /// </summary>
        /// <param name="source">Source <see cref="FileInfo"/> to copy</param>
        /// <param name="destination">Destination <see cref="FileInfo"/> to copy to</param>
        /// <param name="splitSize">Size of split segments</param>
        /// <param name="onWrite">Action will be called everytime theres an advance in bytes with the number of bytes as parameter, to track progress. Can be null.</param>
        public static void Copy(FileInfo source, FileInfo destination, long splitSize, Action<long>? onWrite = null)
        {
            var sourceSplit = new SplitFile(source, splitSize);
            var destSplit = new SplitFile(destination, splitSize);

            int bound;
            if (sourceSplit.SplitInfos.Length > destSplit.SplitInfos.Length)
            {
                // If source is bigger than destination copy from source at last destination pos to end

                bound = destSplit.SplitInfos.Length;

                SplitInfo infoSource = sourceSplit.SplitInfos[bound];

                using var sourceStream = new FileStream(sourceSplit.File.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var destStream = new FileStream(destSplit.File.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                sourceStream.Position = infoSource.StartIndex;
                destStream.Position = infoSource.StartIndex;
                int len = (int)(source.Length - infoSource.StartIndex);

                StreamCopy(sourceStream, destStream, len, onWrite);
            }
            else if (sourceSplit.SplitInfos.Length < destSplit.SplitInfos.Length || sourceSplit.SplitInfos[^1].Length < destSplit.SplitInfos[^1].Length)
            {
                // If destination is bigger than source throw away the diff at the end (set length of dest to src)

                bound = sourceSplit.SplitInfos.Length;

                using var destStream = new FileStream(destSplit.File.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                destStream.SetLength(source.Length);
            }
            else
            {
                // they are the same length w/e

                bound = sourceSplit.SplitInfos.Length;
            }

            // loop to bound, check each split for equality and copy over if needed
            Parallel.For(0, bound, (i) =>
            {
                SplitInfo infoSource = sourceSplit.SplitInfos[i];
                SplitInfo infoDest = destSplit.SplitInfos[i];

                using var sourceStream = new FileStream(sourceSplit.File.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var destStream = new FileStream(destSplit.File.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                sourceStream.Position = infoSource.StartIndex;
                destStream.Position = infoSource.StartIndex;

                // if they are not the same length or not equal copy from src
                if (infoSource.Length != infoDest.Length || !FastCompare.Equals(sourceStream, destStream, infoSource.Length))
                {
                    sourceStream.Position = infoSource.StartIndex;
                    destStream.Position = infoSource.StartIndex;

                    StreamCopy(sourceStream, destStream, infoSource.Length, onWrite);
                }
                else
                {
                    onWrite?.Invoke(infoSource.Length);
                }
            });
        }
    }
}
