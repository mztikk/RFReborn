using System.IO;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Gets the remaining number of bytes in a <see cref="Stream"/> (Length - Position)
        /// </summary>
        /// <param name="s">Stream to use</param>
        public static long Remaining(this Stream s) => s.Length - s.Position;
    }
}
