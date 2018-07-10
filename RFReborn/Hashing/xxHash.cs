using System.Runtime.CompilerServices;

namespace RFReborn.Hashing
{
	public unsafe class xxHash
	{
		private const uint _PRIME32_1 = 2654435761U;
		private const uint _PRIME32_2 = 2246822519U;
		private const uint _PRIME32_3 = 3266489917U;
		private const uint _PRIME32_4 = 668265263U;
		private const uint _PRIME32_5 = 374761393U;

		public static uint Hash(byte[] input, uint seed = 0)
		{
			fixed (byte* inputP = input)
			{
				return Hash(inputP, input.Length, seed);
			}
		}

		public static uint Hash<T>(T input, uint seed = 0) where T : unmanaged
		{
			var p = (void*)&input;
			return Hash(p, sizeof(T), seed);
		}

		public static uint Hash(string input, uint seed = 0)
		{
			fixed (char* p = input)
			{
				return Hash(p, input.Length * sizeof(char), seed);
			}
		}

		public static uint Hash(void* input, int len, uint seed = 0)
		{
			uint h32;
			var p = (byte*)input;
			var bEnd = p + len;

			if (len >= 16)
			{
				var limit = bEnd - 15;

				var v1 = seed + _PRIME32_1 + _PRIME32_2;
				var v2 = seed + _PRIME32_2;
				var v3 = seed + 0;
				var v4 = seed - _PRIME32_1;

				do
				{
					v1 = Round(v1, Get32Bits(p));
					p += 4;
					v2 = Round(v2, Get32Bits(p));
					p += 4;
					v3 = Round(v3, Get32Bits(p));
					p += 4;
					v4 = Round(v4, Get32Bits(p));
					p += 4;
				} while (p < limit);

				h32 = MathR.RotateLeft(v1, 1) + MathR.RotateLeft(v2, 7) + MathR.RotateLeft(v3, 12) + MathR.RotateLeft(v4, 18);
			}
			else
			{
				h32 = seed + _PRIME32_5;
			}

			h32 += (uint)len;

			while (p < bEnd - 4)
			{
				h32 += *((uint*)p) * _PRIME32_3;
				h32 = MathR.RotateLeft(h32, 17) * _PRIME32_4;
				p += 4;
			}
			while (p < bEnd)
			{
				h32 += *p * _PRIME32_5;
				h32 = MathR.RotateLeft(h32, 11) * _PRIME32_1;
				p += 1;
			}

			return Avalanche(h32);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Avalanche(uint h32)
		{
			h32 ^= h32 >> 15;
			h32 *= _PRIME32_2;
			h32 ^= h32 >> 13;
			h32 *= _PRIME32_3;
			h32 ^= h32 >> 16;
			return h32;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Round(uint seed, uint input)
		{
			seed += input * _PRIME32_2;
			seed = MathR.RotateLeft(seed, 13);
			seed += _PRIME32_1;
			return seed;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint Get32Bits(void* ptr) => *(uint*)ptr;

		private static uint Finalize(uint h32, void* ptr, int len)
		{
			var p = (byte*)ptr;
			void Process1()
			{
				h32 += (*p) * _PRIME32_5;
				p++;
				h32 = MathR.RotateLeft(h32, 11) * _PRIME32_1;
			}

			void Process4()
			{
				h32 += Get32Bits(p) * _PRIME32_3;
				p += 4;
				h32 = MathR.RotateLeft(h32, 17) * _PRIME32_4;
			}

			switch (len & 15)
			{
				case 12:
					Process4();
					goto case 8;
				case 8:
					Process4();
					goto case 4;
				case 4:
					Process4();
					return Avalanche(h32);
				case 13:
					Process4();
					goto case 9;
				case 9:
					Process4();
					goto case 5;
				case 5:
					Process4();
					Process1();
					return Avalanche(h32);
				case 14:
					Process4();
					goto case 10;
				case 10:
					Process4();
					goto case 6;
				case 6:
					Process4();
					Process1();
					Process1();
					return Avalanche(h32);
				case 15:
					Process4();
					goto case 11;
				case 11:
					Process4();
					goto case 7;
				case 7:
					Process4();
					goto case 3;
				case 3:
					Process1();
					goto case 2;
				case 2:
					Process1();
					goto case 1;
				case 1:
					Process1();
					goto case 0;
				case 0:
					return Avalanche(h32);
			}

			return h32;
		}
	}
}