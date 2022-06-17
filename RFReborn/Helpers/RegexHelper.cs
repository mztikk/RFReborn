namespace RFReborn.Helpers;

/// <summary>
/// Helper methods for <see cref="Regex"/>
/// </summary>
public static class RegexHelper
{
    /// <summary>
    /// Escapes every string in <paramref name="input"/>
    /// </summary>
    /// <param name="input"><see cref="string"/>s to escape</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> Escape(IEnumerable<string> input)
    {
        foreach (string str in input)
        {
            yield return Regex.Escape(str);
        }
    }
}
