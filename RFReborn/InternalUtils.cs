using RFReborn.Random;

namespace RFReborn
{
	internal static class InternalUtils
	{
		internal static readonly CryptoRandom Rng;

		static InternalUtils()
		{
			Rng = new CryptoRandom();
		}
	}
}