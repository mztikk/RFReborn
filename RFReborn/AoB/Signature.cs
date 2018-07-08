using System;

namespace RFReborn.AoB
{
	/// <summary>
	/// Represents an AoB Signature
	/// </summary>
	public class Signature
	{
		/// <summary>
		/// The byte pattern of the signature
		/// </summary>
		public byte[] Pattern { get; }

		/// <summary>
		/// The mask for the byte pattern of the signature
		/// </summary>
		public string Mask { get; }

		/// <summary>
		/// String representation of the signature in PEiD style i.e. "85 5B ?? 90 ?? ?? BC"
		/// </summary>
		public string Sig { get; }

		/// <summary>
		/// Offset of the signature beginning at the first matching byte
		/// </summary>
		public long Offset { get; }

		#region Constructors
		/// <summary>
		/// Constructs a new instance of <see cref="Signature"/> using a byte pattern and mask, with the offset defaulting to 0
		/// </summary>
		/// <param name="pattern">Byte pattern</param>
		/// <param name="mask">Mask</param>
		public Signature(byte[] pattern, string mask) : this(pattern, mask, 0) { }

		/// <summary>
		/// Constructs a new instance of <see cref="Signature"/> using a byte pattern, mask and an offset
		/// </summary>
		/// <param name="pattern">Byte pattern</param>
		/// <param name="mask">Mask</param>
		/// <param name="offset">Offset</param>
		public Signature(byte[] pattern, string mask, long offset) : this(GetSignatureFromPatternAndMask(pattern, mask), offset) { }

		/// <summary>
		/// Constructs a new instance of <see cref="Signature"/> using a PEiD style signature with the offset defaulting to 0
		/// </summary>
		/// <param name="signature"></param>
		public Signature(string signature) : this(signature, 0) { }

		/// <summary>
		/// Constructs a new instance of <see cref="Signature"/> using a PEiD style signature and an offset
		/// </summary>
		/// <param name="signature"></param>
		/// <param name="offset"></param>
		public Signature(string signature, long offset)
		{
			var (pattern, mask) = GetPatternAndMaskFromSignature(signature);
			this.Pattern = pattern;
			this.Mask = mask;
			this.Offset = offset;
			this.Sig = signature;
		}
		#endregion

		#region Converters
		/// <summary>
		/// Extracts a byte pattern and mask from a PEiD style string signature
		/// </summary>
		/// <param name="signature">PEiD style string signature</param>
		/// <returns>Byte pattern and mask as tuple</returns>
		public static (byte[], string) GetPatternAndMaskFromSignature(string signature)
		{
			var split = signature.Split(' ');
			var bytes = new byte[split.Length];
			var mask = new char[split.Length];
			for (var i = 0; i < split.Length; i++)
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
		/// Constructs a PEiD style signature out of a byte pattern and mask
		/// </summary>
		/// <param name="pattern">Byte pattern</param>
		/// <param name="mask">Mask</param>
		/// <returns>PEiD style string signature</returns>
		/// <exception cref="ArgumentException"></exception>
		public static string GetSignatureFromPatternAndMask(byte[] pattern, string mask)
		{
			if (pattern.Length != mask.Length)
			{
				throw new ArgumentException("Pattern length has to match mask length.");
			}

			var rtn = new string[pattern.Length];
			for (var i = 0; i < rtn.Length; i++)
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
		#endregion
	}
}