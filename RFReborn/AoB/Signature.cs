using System;

namespace RFReborn.AoB
{
    /// <summary>
    /// Represents an AoB Signature.
    /// </summary>
    public unsafe class Signature
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
        /// Constructs a new instance of <see cref="Signature"/> using a byte pattern and mask, with the offset defaulting to 0.
        /// </summary>
        /// <param name="pattern">Byte pattern.</param>
        /// <param name="mask">Mask.</param>
        public Signature(byte[] pattern, string mask) : this(pattern, mask, 0) { }

        /// <summary>
        /// Constructs a new instance of <see cref="Signature"/> using a byte pattern, mask and an offset.
        /// </summary>
        /// <param name="pattern">Byte pattern.</param>
        /// <param name="mask">Mask.</param>
        /// <param name="offset">Offset.</param>
        public Signature(byte[] pattern, string mask, long offset) : this(GetSignatureFromPatternAndMask(pattern, mask), offset) { }

        /// <summary>
        /// Constructs a new instance of <see cref="Signature"/> using a PEiD style signature with the offset defaulting to 0.
        /// </summary>
        /// <param name="signature"></param>
        public Signature(string signature) : this(signature, 0) { }

        /// <summary>
        /// Constructs a new instance of <see cref="Signature"/> using a PEiD style signature and an offset.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="offset"></param>
        public Signature(string signature, long offset)
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
        public static (byte[] aob, string mask) GetPatternAndMaskFromSignature(string signature)
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
            //var split = signature.Split(' ');
            byte[] bytes = new byte[split.Length];
            char* mask = stackalloc char[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i][0] == '?' || split[i][1] == '?')
                {
                    bytes[i] = 0;
                    mask[i] = '?';
                }
                else
                {
                    bytes[i] = Convert.ToByte(new string(new[] { split[i][0], split[i][1] }), 16);
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

            //var split = StringR.SplitN(signature, 2);
            //return string.Join(" ", split);
            return StringR.InSplit(signature, 2, " ");
        }
        #endregion Converters
    }
}
