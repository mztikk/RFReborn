namespace RFReborn.Files;

/// <summary>
/// Options to enumerate the FileSystem
/// </summary>
public enum FileSystemEnumeration : byte
{
    /// <summary>
    /// Enumerate only Files
    /// </summary>
    FilesOnly = 0,

    /// <summary>
    /// Enumerate only Directories
    /// </summary>
    DirectoriesOnly = 1,

    /// <summary>
    /// Enumerate Files and Directories
    /// </summary>
    FilesAndDirectories = 2
}
