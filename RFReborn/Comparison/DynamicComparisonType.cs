namespace RFReborn.Comparison;

/// <summary>
/// Mode of dynamic comparison between objects
/// </summary>
public enum DynamicComparisonType
{
    /// <summary>
    /// Fully match everything
    /// </summary>
    Full,

    /// <summary>
    /// Match any single
    /// </summary>
    Any,

    /// <summary>
    /// Match only non null values
    /// </summary>
    NonNull
}
