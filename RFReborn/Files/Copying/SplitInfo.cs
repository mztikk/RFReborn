namespace RFReborn.Files.Copying
{
    /// <summary>
    /// Class that represents a split part with a <see cref="StartIndex"/>, <see cref="Length"/> and <see cref="EndIndex"/>
    /// </summary>
    internal class SplitInfo
    {
        public SplitInfo(long startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
            EndIndex = startIndex + length;
        }

        public long StartIndex { get; }
        public int Length { get; }
        public long EndIndex { get; }
    }
}
