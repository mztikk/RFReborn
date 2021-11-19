namespace RFReborn.Files.FileCollector.Modules;

/// <summary>
/// Interface for Modules that define if a path should be collected / taken or skipped
/// </summary>
public interface ICollectingModule
{
    /// <summary>
    /// If the path should be skipped
    /// </summary>
    /// <param name="path">path to examine</param>
    bool Skip(string path);

    /// <summary>
    /// If the path should be taken
    /// </summary>
    /// <param name="path">path to examine</param>
    bool Take(string path);
}
