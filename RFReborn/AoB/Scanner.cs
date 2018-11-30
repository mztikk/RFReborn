using System.IO;

namespace RFReborn.AoB
{
    /// <summary>
    /// Searches for <see cref="Signature"/> in various regions.
    /// </summary>
    public static class Scanner
    {
        /// <summary>
        /// Size of buffer to be used when reading from streams.
        /// </summary>
        private const int _BufferSize = 4048;

        #region FindSig
        #region Overloads
        /// <summary>
        /// Searches for the byte pattern + mask inside of a stream.
        /// </summary>
        /// <param name="searchRegion">The stream to be searched.</param>
        /// <param name="pattern">The byte pattern to search for.</param>
        /// <param name="mask">The mask for the pattern.</param>
        /// <returns>The zero-based index position of <paramref name="pattern"/> and <paramref name="mask"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(Stream searchRegion, byte[] pattern, string mask) => FindSignature(searchRegion, new Signature(pattern, mask));

        /// <summary>
        /// Searches for the byte pattern + mask with an offset inside of a stream.
        /// </summary>
        /// <param name="searchRegion">The stream to be searched.</param>
        /// <param name="pattern">The byte pattern to search for.</param>
        /// <param name="mask">The mask for the pattern.</param>
        /// <param name="offset">Offset to rebase the position.</param>
        /// <returns>The zero-based index position of <paramref name="pattern"/> and <paramref name="mask"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(Stream searchRegion, byte[] pattern, string mask, long offset) => FindSignature(searchRegion, new Signature(pattern, mask, offset));

        /// <summary>
        /// Searches for a PEiD style string signature inside of a stream.
        /// </summary>
        /// <param name="searchRegion">The stream to be searched.</param>
        /// <param name="signature">PEiD style string signature.</param>
        /// <returns>The zero-based index position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(Stream searchRegion, string signature) => FindSignature(searchRegion, new Signature(signature));

        /// <summary>
        /// Searches for a PEiD style string signature with an offset inside of a stream.
        /// </summary>
        /// <param name="searchRegion">The stream to be searched.</param>
        /// <param name="signature">PEiD style string signature.</param>
        /// <param name="offset">Offset to rebase the position.</param>
        /// <returns>The zero-based index position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(Stream searchRegion, string signature, long offset) => FindSignature(searchRegion, new Signature(signature, offset));

        /// <summary>
        /// Searches for the byte pattern + mask inside of a byte array.
        /// </summary>
        /// <param name="searchRegion">The region to be searched.</param>
        /// <param name="pattern">The byte pattern to search for.</param>
        /// <param name="mask">The mask for the pattern.</param>
        /// <returns>The zero-based index position of <paramref name="pattern"/> and <paramref name="mask"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(byte[] searchRegion, byte[] pattern, string mask) => FindSignature(searchRegion, new Signature(pattern, mask));

        /// <summary>
        /// Searches for the byte pattern + mask with an offset inside of a byte array.
        /// </summary>
        /// <param name="searchRegion">The region to be searched.</param>
        /// <param name="pattern">The byte pattern to search for.</param>
        /// <param name="mask">The mask for the pattern.</param>
        /// <param name="offset">Offset to rebase the position.</param>
        /// <returns>The zero-based index position of <paramref name="pattern"/> and <paramref name="mask"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(byte[] searchRegion, byte[] pattern, string mask, long offset) => FindSignature(searchRegion, new Signature(pattern, mask, offset));

        /// <summary>
        /// Searches for the PEiD style string signature inside of a byte array.
        /// </summary>
        /// <param name="searchRegion">The region to be searched.</param>
        /// <param name="signature">The <see cref="Signature"/> to search for.</param>
        /// <returns>The zero-based index position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(byte[] searchRegion, string signature) => FindSignature(searchRegion, new Signature(signature));

        /// <summary>
        /// Searches for the PEiD style string signature + offset inside of a byte array.
        /// </summary>
        /// <param name="searchRegion">The region to be searched.</param>
        /// <param name="signature">The <see cref="Signature"/> to search for.</param>
        /// <param name="offset">Offset to rebase the position.</param>
        /// <returns>The zero-based index position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(byte[] searchRegion, string signature, long offset) => FindSignature(searchRegion, new Signature(signature, offset));
        #endregion

        #region Actual Implementations
        /// <summary>
        /// Searches for a given <see cref="Signature"/> inside of a stream.
        /// </summary>
        /// <param name="searchRegion">The stream to be searched.</param>
        /// <param name="signature">The <see cref="Signature"/> to search for.</param>
        /// <returns>The offset position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(Stream searchRegion, Signature signature)
        {
            var buffer = new byte[_BufferSize];
            int readByteCount;
            while ((readByteCount = searchRegion.Read(buffer, 0, buffer.Length)) != 0)
            {
                var find = FindSignature(buffer, signature);
                if (find != -1)
                {
                    return searchRegion.Position - readByteCount + find;
                }
            }

            return -1;
        }

        /// <summary>
        /// Searches for a given <see cref="Signature"/> inside of a byte array.
        /// </summary>
        /// <param name="searchRegion">The region to be searched.</param>
        /// <param name="signature">The <see cref="Signature"/> to search for.</param>
        /// <returns>The zero-based index position of <paramref name="signature"/> if that <see cref="Signature"/> is found, or -1 if it is not.</returns>
        public static long FindSignature(byte[] searchRegion, Signature signature)
        {
            var firstIndex = signature.Mask.IndexOf('x');
            var firstItem = signature.Pattern[firstIndex];

            for (var i = 0; i < searchRegion.Length - signature.Pattern.Length; i++)
            {
                if (searchRegion[i + firstIndex] != firstItem)
                {
                    continue;
                }

                if (CheckMask(i, searchRegion, signature.Pattern, signature.Mask))
                {
                    return i + signature.Offset;
                }
            }

            return -1;
        }

        /// <summary>
        /// Checks if the byte pattern + mask match the searchregion at the specified index.
        /// </summary>
        /// <param name="index">Index in the search region.</param>
        /// <param name="searchRegion">Space to search.</param>
        /// <param name="pattern">pattern to match.</param>
        /// <param name="mask">mask for the pattern.</param>
        /// <returns>TRUE if it matches; FALSE otherwise.</returns>
        private static bool CheckMask(int index, byte[] searchRegion, byte[] pattern, string mask)
        {
            for (var i = 0; i < pattern.Length; i++)
            {
                if (mask[i] == '?')
                {
                    continue;
                }

                if (mask[i] == 'x' && pattern[i] != searchRegion[index + i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
        #endregion
    }
}
