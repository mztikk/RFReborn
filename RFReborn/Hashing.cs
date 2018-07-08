using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Hash
{
	/// <summary>
	/// Provides various hashing functionalities.
	/// </summary>
	public static class Hashing
	{
		/// <summary>
		/// List of all valid hash names
		/// </summary>
		public static ReadOnlyCollection<string> ValidHashes = new ReadOnlyCollection<string>(new List<string>() { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" });

		/// <summary>
		/// Gets the name of the hash as represented in <see cref="ValidHashes"/>. Turns "md5" into "MD5" for example.
		/// </summary>
		/// <param name="name">Name of the hash to get the actual key</param>
		/// <returns>Returns the actual name of the hash or <see cref="string.Empty"/> if it does not exist.</returns>
		public static string GetHashName(string name)
		{
			foreach (var hash in ValidHashes)
			{
				if (hash.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return hash;
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Hashes the stream with the given hash algorithm, uses <see cref="Encoding.UTF8"/> to encode the input string to bytes.
		/// </summary>
		/// <param name="hashName">Hash algorithm to use</param>
		/// <param name="input">Input to hash</param>
		/// <returns>Hex string of the hashed input</returns>
		public static string Hash(string hashName, string input) => Hash(hashName, input, Encoding.UTF8);

		/// <summary>
		/// Hashes the stream with the given hash algorithm.
		/// </summary>
		/// <param name="hashName">Hash algorithm to use</param>
		/// <param name="input">Input to hash</param>
		/// <param name="encoding">Encoding to use to encode the input string to bytes</param>
		/// <returns>Hex string of the hashed input</returns>
		public static string Hash(string hashName, string input, Encoding encoding) => Hash(hashName, encoding.GetBytes(input));

		/// <summary>
		/// Hashes the stream with the given hash algorithm.
		/// </summary>
		/// <param name="hashName">Hash algorithm to use</param>
		/// <param name="input">Input to hash</param>
		/// <returns>Hex string of the hashed input</returns>
		public static string Hash(string hashName, byte[] input)
		{
			if (!ValidHashes.Contains(hashName))
			{
				throw new ArgumentException(nameof(hashName), "Hash does not exist");
			}

			using (var hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashName))
			{
				var hashedBytes = hashAlgorithm.ComputeHash(input);
				return Stringify(hashedBytes);
			}
		}

		/// <summary>
		/// Hashes the stream with the given hash algorithm.
		/// </summary>
		/// <param name="hashName">Hash algorithm to use</param>
		/// <param name="input">Input to hash</param>
		/// <returns>Hex string of the hashed input</returns>
		public static string Hash(string hashName, Stream input)
		{
			if (!ValidHashes.Contains(hashName))
			{
				throw new ArgumentException(nameof(hashName), "Hash does not exist");
			}

			using (var hashAlgorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashName))
			{
				var hashedBytes = hashAlgorithm.ComputeHash(input);
				return Stringify(hashedBytes);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string Stringify(byte[] hexBytes)
		{
			var rtn = new StringBuilder();
			for (var i = 0; i < hexBytes.Length; i++)
			{
				rtn.Append(hexBytes[i].ToString("X2"));
			}
			return rtn.ToString();
		}
	}
}