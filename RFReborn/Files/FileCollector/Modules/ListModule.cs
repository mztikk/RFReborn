using System;
using System.Collections.Generic;
using RFReborn.Extensions;

namespace RFReborn.Files.FileCollector.Modules;

/// <summary>
/// <see cref="ICollectingModule"/> implementation for Allow and Blocklist wildcard patterns
/// </summary>
public class ListModule : ICollectingModule
{
    private readonly IEnumerable<string> _allowPatterns;
    private readonly IEnumerable<string> _blockPatterns;

    private readonly bool _allowAny;
    private readonly bool _blockAny;

    /// <summary>
    /// Constructs a new <see cref="ListModule"/> with given patterns
    /// </summary>
    /// <param name="allowPatterns">patterns to decide if a path should be taken</param>
    /// <param name="blockPatterns">patterns to decide if a path should be skipped</param>
    public ListModule(IEnumerable<string> allowPatterns, IEnumerable<string> blockPatterns)
    {
        _allowPatterns = allowPatterns;
        _blockPatterns = blockPatterns;

        _allowAny = _allowPatterns.Any();
        _blockAny = _blockPatterns.Any();
    }

    /// <inheritdoc />
    public bool Skip(string path)
    {
        // If Blocklist is empty don't skip anything
        if (!_blockAny)
        {
            return false;
        }

        return WildcardHelper.IsMatch(path, _blockPatterns);
    }

    /// <inheritdoc />
    public bool Take(string path)
    {
        // If Allowlist is empty take everything
        if (!_allowAny)
        {
            return true;
        }

        return WildcardHelper.IsMatch(path, _allowPatterns);
    }
}

/// <summary>
/// Extensions class for ListModule and FileCollector
/// </summary>
public static class ListModuleExtensions
{
    /// <summary>
    /// Adds a new <see cref="ListModule"/> to a <see cref="FileCollector"/> with given patterns
    /// </summary>
    /// <param name="fileCollector">FileCollector to add module to</param>
    /// <param name="allowPatterns">patterns to decide if a path should be taken</param>
    /// <param name="blockPatterns">patterns to decide if a path should be skipped</param>
    public static FileCollector AddListModule(this FileCollector fileCollector, IEnumerable<string> allowPatterns, IEnumerable<string> blockPatterns)
    {
        fileCollector.AddModule(new ListModule(allowPatterns, blockPatterns));
        return fileCollector;
    }

    /// <summary>
    /// Adds a new <see cref="ListModule"/> to a <see cref="FileCollector"/> with given patterns
    /// </summary>
    /// <param name="fileCollector">FileCollector to add module to</param>
    /// <param name="blockPatterns">patterns to decide if a path should be skipped</param>
    public static FileCollector AddListModule(this FileCollector fileCollector, IEnumerable<string> blockPatterns)
    {
        fileCollector.AddModule(new ListModule(Array.Empty<string>(), blockPatterns));
        return fileCollector;
    }
}
