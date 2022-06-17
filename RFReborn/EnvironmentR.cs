namespace RFReborn;

/// <summary>
/// Contains various functions about the current environment and platform.
/// </summary>
public static class EnvironmentR
{
    private static char PathSeparator()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return ';';
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return ':';
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ':';
        }

        return ';';
    }

    /// <summary>
    /// Returns all Directories in the PATH Environment Variable
    /// </summary>
    public static string[]? PathDirectories() => Environment.GetEnvironmentVariable("PATH")?.Split(PathSeparator());

    /// <summary>
    /// Enumerates all Directories in the PATH Environment Variable which exist
    /// </summary>
    public static IEnumerable<string> ExistingPathDirectories() => (PathDirectories() ?? Array.Empty<string>()).Where(Directory.Exists);

    /// <summary>
    /// Searches for a file name in all PATH Directories
    /// </summary>
    /// <param name="fileName">Filename to search for</param>
    /// <returns>Path to the file if found; <see langword="null"/> otherwise</returns>
    public static string? FindFileInPath(string fileName)
    {
        foreach (string directory in ExistingPathDirectories().Distinct())
        {
            foreach (string file in FileUtils.EnumerateFiles(directory))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (Path.GetFileName(file).Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return file;
                    }
                }
                else
                {
                    if (Path.GetFileName(file) == fileName)
                    {
                        return file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file) == fileName)
                    {
                        return file;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Searches for a file name in all PATH Directories and enumerates all results
    /// </summary>
    /// <param name="fileName">Filename to search for</param>
    /// <returns>Path to the files</returns>
    public static IEnumerable<string> FindFilesInPath(string fileName)
    {
        foreach (string directory in ExistingPathDirectories().Distinct())
        {
            foreach (string file in FileUtils.EnumerateFiles(directory))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    if (Path.GetFileName(file).Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        yield return file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file).Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        yield return file;
                    }
                }
                else
                {
                    if (Path.GetFileName(file) == fileName)
                    {
                        yield return file;
                    }
                    else if (Path.GetFileNameWithoutExtension(file) == fileName)
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}
