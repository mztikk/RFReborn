using System;
using System.Globalization;

namespace RFReborn.AoB
{
    /// <summary>
    /// Represents an AoB Signature.
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// The byte pattern of the signature.
        /// </summary>
        public byte[] Pattern { get; }

        /// <summary>
        /// The mask for the byte pattern of the signature.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// String representation of the signature in PEiD style i.e. "85 5B ?? 90 ?? ?? BC".
        /// </summary>
        public string Sig { get; }

        /// <summary>
        /// Offset of the signature beginning at the first matching byte.
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// Index of the first wildcard in <see cref="Mask"/>
        /// </summary>
        public int FirstWildcard { get; }

        /// <summary>
        /// Index of the first byte in <see cref="Mask"/>
        /// </summary>
        public int FirstByte { get; }

        /// <summary>
        /// Length of the Signature / Number of bytes
        /// </summary>
        public long Length { get; }

        #region Constructors

        /// <summary>
        /// Constructs a new instance of <see cref="Signature"/> using a byte pattern, mask and an offset.
        /// </summary>
        /// <param name="pattern">Byte pattern.</param>
        /// <param name="mask">Mask.</param>
        /// <param name="offset">Offset.</param>
        public Signature(byte[] pattern, string mask, long offset = 0) : this(GetSignatureFromPatternAndMask(pattern, mask), offset) { }

        /// <summary>
        /// Constructs a new instance of <see cref="Signature"/> using a PEiD style signature and an offset.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="offset"></param>
        public Signature(string signature, long offset = 0)
        {
            (byte[] pattern, string mask) = GetPatternAndMaskFromSignature(signature);
            Pattern = pattern;
            Mask = mask;
            Offset = offset;
            Sig = Standardize(signature);
            FirstWildcard = mask.IndexOf('?');
            FirstByte = mask.IndexOf('x');
            Length = pattern.LongLength;
        }
        #endregion Constructors

        #region Converters
        /// <summary>
        /// Extracts a byte pattern and mask from a PEiD style string signature.
        /// </summary>
        /// <param name="signature">PEiD style string signature.</param>
        /// <returns>Byte pattern and mask as tuple.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static unsafe (byte[] aob, string mask) GetPatternAndMaskFromSignature(string signature)
        {
            // remove whitespace and split every 2 chars so we can support sigs that dont even have whitespace in the first place or are not formatted with whitespace after every byte/wildcard
            // 0F ?? AE ?? CC |standard
            // 0F??AE ?? CC |works now also
            signature = StringR.RemoveWhitespace(signature);
            if (signature.Length % 2 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(signature), "signature length(excluding whitespace) must be divisible by 2, make sure to prepend bytes with 0 if necessary and make wildcards full ?? instead of single ?");
            }

            string[] split = StringR.SplitN(signature, 2);
            byte[] bytes = new byte[split.Length];
            char* mask = stackalloc char[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                // check if current or next is a wildcard because of halfbyte masking 0xFF | 0x?F | 0xF? | 0x??
                if (split[i][0] == '?' || split[i][1] == '?')
                {
                    bytes[i] = 0;
                    mask[i] = '?';
                }
                // consider everything that is not a wildcard a byte
                else
                {
                    bytes[i] = (byte)int.Parse(split[i], NumberStyles.HexNumber);
                    mask[i] = 'x';
                }
            }

            return (bytes, new string(mask));
        }

        /// <summary>
        /// Constructs a PEiD style signature out of a byte pattern and mask.
        /// </summary>
        /// <param name="pattern">Byte pattern.</param>
        /// <param name="mask">Mask.</param>
        /// <returns>PEiD style string signature.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetSignatureFromPatternAndMask(byte[] pattern, string mask)
        {
            if (pattern.Length != mask.Length)
            {
                throw new ArgumentException("Pattern length has to match mask length.");
            }

            string[] rtn = new string[pattern.Length];
            for (int i = 0; i < rtn.Length; i++)
            {
                if (mask[i] == 'x')
                {
                    rtn[i] = pattern[i].ToString("X2");
                }
                else if (mask[i] == '?')
                {
                    rtn[i] = "??";
                }
            }

            return string.Join(" ", rtn);
        }

        /// <summary>
        /// Returns the standardized format of a signature, delimited with a space after every byte, with bytes being 2 chars, eg: "0F ?? AE ?? CC"
        /// </summary>
        /// <param name="signature">signature to format</param>
        /// <returns>formatted signature</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Standardize(string signature)
        {
            signature = StringR.RemoveWhitespace(signature);
            if (signature.Length % 2 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(signature), "signature length(excluding whitespace) must be divisible by 2, make sure to prepend bytes with 0 if necessary and make wildcards full ?? instead of single ?");
            }

            return StringR.InSplit(signature, 2, " ").ToUpperInvariant();
        }
        #endregion Converters

        /// <summary>
        /// Converts a PEiD style signature string to a <see cref="Signature"/> object
        /// </summary>
        /// <param name="signature">PEiD style signature string to convert</param>
        public static explicit operator Signature(string signature) => new(signature);
    }
}
