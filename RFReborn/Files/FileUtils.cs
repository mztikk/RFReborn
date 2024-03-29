﻿using System;
using RFReborn.AoB;

namespace RFReborn.Files;

/// <summary>
///     Utilities to interact with the FileSystem
/// </summary>
public static class FileUtils
{
    private const int InitialCapacity = 64;
    private const int DefaultFileStreamBufferSize = 4096;

    /// <summary>
    ///     Checks if there is any File inside a root path
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns>Returns <see langword="true" /> if there is any File; <see langword="false" /> otherwise.</returns>
    public static bool AnyFile(string root)
    {
        Stack<string> dirs = new(InitialCapacity);
        if (!Directory.Exists(root))
        {
            throw new ArgumentException($"Directory does not exist. '{root}'", nameof(root));
        }

        dirs.Push(root);
        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();
            string[] files;
            try
            {
                files = Directory.GetFiles(currentDir);
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (DirectoryNotFoundException)
            {
                continue;
            }

            if (files.Length > 0)
            {
                return true;
            }

            string[] subDirs;
            try
            {
                subDirs = Directory.GetDirectories(currentDir);
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (DirectoryNotFoundException)
            {
                continue;
            }

            subDirs.Call(dirs.Push);
        }

        return false;
    }

    #region GetFiles

    /// <summary>
    ///     Enumerates the tree of a root path and yields all files
    /// </summary>
    /// <param name="root">Path to start walking</param>
    /// <param name="skipDirectory">
    ///     <see cref="Func{T, TResult}" /> to check each <see cref="DirectoryInfo" />; if this returns
    ///     true it will be skipped and not further traversed
    /// </param>
    /// <returns></returns>
    public static IEnumerable<string> GetFiles(string root, Func<DirectoryInfo, bool>? skipDirectory)
    {
        Stack<string> dirs = new(InitialCapacity);
        if (!Directory.Exists(root))
        {
            throw new ArgumentException($"Directory does not exist. '{root}'", nameof(root));
        }

        dirs.Push(NormalizePath(root));
        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();

            DirectoryInfo di = new(currentDir);
            if (skipDirectory is null || !skipDirectory(di))
            {
                IEnumerable<string> subDirs;
                try
                {
                    subDirs = Directory.EnumerateDirectories(currentDir);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                subDirs.Call(NormalizePath).Call(dirs.Push);

                IEnumerable<string> files;
                try
                {
                    files = Directory.EnumerateFiles(currentDir);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                foreach (string file in files.Call(NormalizePath))
                {
                    yield return file;
                }
            }
        }
    }

    #endregion GetFiles

    /// <summary>
    ///     Enumerates all files in a directory
    /// </summary>
    /// <param name="directory">Path of the directory to enumerate</param>
    /// <returns></returns>
    public static IEnumerable<string> EnumerateFiles(string directory)
    {
        IEnumerable<string> files;
        try
        {
            files = Directory.EnumerateFiles(directory);
        }
        catch (UnauthorizedAccessException)
        {
            yield break;
        }
        catch (DirectoryNotFoundException)
        {
            yield break;
        }

        foreach (string file in files.Call(NormalizePath))
        {
            yield return file;
        }
    }

    /// <summary>
    ///     Finds files by walking the root and wildcard matching the path and alt path with a given mask
    /// </summary>
    /// <param name="root">Root path to start walking</param>
    /// <param name="pattern">Wildcard mask to match filepath</param>
    public static IEnumerable<string> FindFilesByMatch(string root, string pattern)
    {
        foreach (string file in Walk(root, FileSystemEnumeration.FilesOnly))
        {
            string[] tests = { Path.GetFileName(file), file, GetAltPath(file) };
            foreach (string test in tests)
            {
                if (StringR.WildcardMatch(test, pattern))
                {
                    yield return file;
                    break;
                }
            }
        }
    }

    /// <summary>
    ///     Finds files by walking the root and matching the <see cref="Signature" />
    /// </summary>
    /// <param name="root">Root path to start walking</param>
    /// <param name="signature"><see cref="Signature" /> to match</param>
    /// <param name="continueOnException">
    ///     If <see langword="true" /> continues on exception, if <see langword="false" /> throws
    ///     them. May happen when reading from files
    /// </param>
    public static IEnumerable<string> FindFilesBySignature(string root, Signature signature,
        bool continueOnException = true)
    {
        foreach (string file in Walk(root, FileSystemEnumeration.FilesOnly))
        {
            bool findFlag = false;
            try
            {
                using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (Scanner.FindSignature(fs, signature) != -1)
                {
                    findFlag = true;
                }
            }
            catch (Exception) when (continueOnException)
            {
                continue;
            }

            if (findFlag)
            {
                yield return file;
            }
        }
    }

    /// <summary>
    ///     Copies all files from <paramref name="files" /> based on <paramref name="baseOrigin" />
    ///     <see cref="DirectoryInfo" /> relative path to <paramref name="destination" />
    /// </summary>
    /// <param name="baseOrigin">Base Origin for relative path</param>
    /// <param name="destination">Destination to copy to</param>
    /// <param name="files">Files to copy</param>
    /// <param name="overwrite">Overwrite files</param>
    public static void Copy(DirectoryInfo baseOrigin, DirectoryInfo destination, IEnumerable<FileInfo> files,
        bool overwrite = true)
    {
        if (!destination.Exists)
        {
            destination.Create();
        }

        foreach (FileInfo file in files)
        {
            if (!file.Exists)
            {
                throw new ArgumentException($"File does not exist. '{file}'", nameof(files));
            }

            string relPath = Path.GetRelativePath(baseOrigin.FullName, file.FullName);
            string newPath = Path.Combine(destination.FullName, relPath);
            FileInfo newFile = new(newPath);
            if (newFile.Directory is { Exists: false })
            {
                newFile.Directory.Create();
            }

            CopyTo(file, newFile, overwrite);
            //file.CopyTo(newPath, overwrite);
        }
    }

    /// <summary>
    ///     Makes path of a file to a new base destination relative from a base origin
    ///     <para />
    ///     For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file
    ///     would translate to D:\newdir\diffsub\subdir\abc\bin.exe
    /// </summary>
    /// <param name="baseOrigin">Base Origin</param>
    /// <param name="baseDestination">Base Destination</param>
    /// <param name="file">File to make new path</param>
    public static string MakePath(DirectoryInfo baseOrigin, DirectoryInfo baseDestination, FileInfo file)
    {
        string relPath = Path.GetRelativePath(baseOrigin.FullName, file.FullName);
        return NormalizePath(Path.Combine(baseDestination.FullName, relPath));
    }

    /// <summary>
    ///     Makes path of a file to a new base destination relative from a base origin
    ///     <para />
    ///     For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file
    ///     would translate to D:\newdir\diffsub\subdir\abc\bin.exe
    /// </summary>
    /// <param name="baseOrigin">Base Origin</param>
    /// <param name="baseDestination">Base Destination</param>
    /// <param name="file">File to make new path</param>
    public static string MakePath(string baseOrigin, string baseDestination, string file)
    {
        string relPath = Path.GetRelativePath(baseOrigin, file);
        return NormalizePath(Path.Combine(baseDestination, relPath));
    }

    /// <summary>
    ///     Makes path of a file to a new base destination relative from a base origin
    ///     <para />
    ///     For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file
    ///     would translate to D:\newdir\diffsub\subdir\abc\bin.exe
    /// </summary>
    /// <param name="baseOrigin">Base Origin</param>
    /// <param name="baseDestination">Base Destination</param>
    /// <param name="file">File to make new path</param>
    public static string MakePath(DirectoryInfo baseOrigin, DirectoryInfo baseDestination, string file)
    {
        string relPath = Path.GetRelativePath(baseOrigin.FullName, file);
        return NormalizePath(Path.Combine(baseDestination.FullName, relPath));
    }

    /// <summary>
    ///     Copies a file from <paramref name="file" /> based on <paramref name="baseOrigin" /> <see cref="DirectoryInfo" />
    ///     relative path to <paramref name="destination" />
    /// </summary>
    /// <param name="baseOrigin">Base Origin for relative path</param>
    /// <param name="destination">Destination to copy to</param>
    /// <param name="file">File to copy</param>
    /// <param name="overwrite">Overwrite files</param>
    public static void Copy(DirectoryInfo baseOrigin, DirectoryInfo destination, FileInfo file,
        bool overwrite = true)
    {
        if (!destination.Exists)
        {
            destination.Create();
        }

        if (!file.Exists)
        {
            throw new ArgumentException($"File does not exist. '{file}'", nameof(file));
        }

        string relPath = Path.GetRelativePath(baseOrigin.FullName, file.FullName);
        string newPath = Path.Combine(destination.FullName, relPath);
        FileInfo newFile = new(newPath);
        if (newFile.Directory is { Exists: false })
        {
            newFile.Directory.Create();
        }

        CopyTo(file, newFile, overwrite);
        //file.CopyTo(newPath, overwrite);
    }

    /// <summary>
    ///     Copies all files and directories from <paramref name="source" /> to <paramref name="destination" />
    /// </summary>
    /// <param name="source">Source to copy</param>
    /// <param name="destination">Destination to copy to</param>
    public static void Copy(this DirectoryInfo source, DirectoryInfo destination)
    {
        if (!destination.Exists)
        {
            destination.Create();
        }

        Walk(source.FullName,
            fi =>
            {
                string relPath = Path.GetRelativePath(source.FullName, fi.FullName);
                string newPath = Path.Combine(destination.FullName, relPath);
                FileInfo newFile = new(newPath);
                if (newFile.Directory is { Exists: false })
                {
                    newFile.Directory.Create();
                }

                CopyTo(fi, newFile, false);
                //fi.CopyTo(newPath, false);
            });
    }

    /// <summary>
    ///     Copies files from <paramref name="source" /> <see cref="DirectoryInfo" /> to <paramref name="target" />
    ///     <see cref="DirectoryInfo" />
    /// </summary>
    /// <param name="source">Source directory</param>
    /// <param name="target">Target directory</param>
    /// <param name="onlyDiffFiles">Copy only files that are different</param>
    /// <returns>Relative path of copied files</returns>
    public static IEnumerable<string> Copy(DirectoryInfo source, DirectoryInfo target, bool onlyDiffFiles)
    {
        if (!target.Exists)
        {
            target.Create();
        }

        IEnumerable<string> files = onlyDiffFiles
            ? GetDiffFiles(source, target)
            : Walk(source.FullName, FileSystemEnumeration.FilesOnly);
        foreach (string file in files)
        {
            string filePath = Path.Combine(source.FullName, file);
            string newFilePath = Path.Combine(target.FullName, file);
            if (!File.Exists(newFilePath))
            {
                Directory.CreateDirectory(Directory.GetParent(newFilePath).FullName);
                File.Create(newFilePath).Dispose();
            }

            //File.Copy(filePath, newFilePath);
            CopyTo(filePath, newFilePath, false);
            yield return file;
        }
    }

    /// <summary>
    ///     Returns the relative path of all files which are different between the source and target
    ///     <see cref="DirectoryInfo" />
    /// </summary>
    /// <param name="source">Source path</param>
    /// <param name="target">Target path</param>
    /// <returns>Relative path of different files</returns>
    public static IEnumerable<string> GetDiffFiles(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (string sourceFile in Walk(source.FullName, FileSystemEnumeration.FilesOnly))
        {
            string relativePath = Path.GetRelativePath(source.FullName, sourceFile);
            if (!target.Exists)
            {
                yield return relativePath;
            }
            else
            {
                string targetFile = Path.Combine(target.FullName, relativePath);
                if (!File.Exists(targetFile) || AreDifferent(targetFile, sourceFile))
                {
                    yield return relativePath;
                }
            }
        }
    }

    /// <summary>
    ///     Copies a file to a destination
    /// </summary>
    /// <param name="source">Source file to copy</param>
    /// <param name="destination">Destination to copy to</param>
    /// <param name="overwrite">Allow overwriting of destination file or not</param>
    /// <param name="onWrite">
    ///     Action will be called everytime theres an advance in bytes with the number of bytes as parameter,
    ///     to track progress. Can be null.
    /// </param>
    /// <exception cref="IOException">If the destination already exists and overwriting is not allowed</exception>
    public static void Copy(FileInfo source, FileInfo destination, bool overwrite = true,
        Action<long>? onWrite = null) => CopyTo(source, destination, overwrite, onWrite);

    /// <inheritdoc cref="Copy(System.IO.FileInfo,System.IO.FileInfo,bool,System.Action{long}?)" />
    /// <summary>
    ///     Asynchronously Copies a file to a destination
    /// </summary>
    public static async Task CopyAsync(FileInfo source, FileInfo destination, bool overwrite = true,
        Action<long>? onWrite = null) => await CopyToAsync(source, destination, overwrite, onWrite);

    /// <inheritdoc cref="Copy(System.IO.FileInfo,System.IO.FileInfo,bool,System.Action{long}?)" />
    /// <summary>
    ///     Asynchronously Copies a file to a destination
    /// </summary>
    public static async Task CopyAsync(FileInfo source, FileInfo destination, bool overwrite = true,
        Func<long, Task>? onWrite = null) => await CopyToAsync(source, destination, overwrite, onWrite);

    /// <summary>
    ///     Compares two files for inequality
    /// </summary>
    /// <param name="left">First file to compare</param>
    /// <param name="right">Second file to compare</param>
    /// <returns><see langword="true" /> if files are different, <see langword="false" /> otherwise</returns>
    public static bool AreDifferent(this FileInfo left, FileInfo right)
    {
        using FileStream leftData = left.OpenRead();
        using FileStream rightData = right.OpenRead();
        return FastCompare.NotEquals(leftData, rightData);
    }

    /// <summary>
    ///     Compares two files for inequality
    /// </summary>
    /// <param name="left">First file to compare</param>
    /// <param name="right">Second file to compare</param>
    /// <returns><see langword="true" /> if files are different, <see langword="false" /> otherwise</returns>
    public static bool AreDifferent(string left, string right)
    {
        using FileStream leftData = File.OpenRead(left);
        using FileStream rightData = File.OpenRead(right);
        return FastCompare.NotEquals(leftData, rightData);
    }

    /// <summary>
    ///     Compares two files for equality
    /// </summary>
    /// <param name="left">First file to compare</param>
    /// <param name="right">Second file to compare</param>
    /// <returns><see langword="true" /> if files are equal, <see langword="false" /> otherwise</returns>
    public static bool AreEqual(this FileInfo left, FileInfo right)
    {
        using FileStream leftData = left.OpenRead();
        using FileStream rightData = right.OpenRead();
        return FastCompare.Equals(leftData, rightData);
    }

    /// <summary>
    ///     Compares two files for equality
    /// </summary>
    /// <param name="left">First file to compare</param>
    /// <param name="right">Second file to compare</param>
    /// <returns><see langword="true" /> if files are equal, <see langword="false" /> otherwise</returns>
    public static bool AreEqual(string left, string right)
    {
        using FileStream leftData = File.OpenRead(left);
        using FileStream rightData = File.OpenRead(right);
        return FastCompare.Equals(leftData, rightData);
    }

    /// <summary>
    ///     Clears a <see cref="DirectoryInfo" />, deleting all files inside
    /// </summary>
    /// <param name="directoryInfo"><see cref="DirectoryInfo" /> to clear</param>
    /// <exception cref="ArgumentException">Directory does not exist</exception>
    public static void Clear(this DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            throw new ArgumentException("Directory does not exist");
        }

        Walk(directoryInfo.FullName,
            (FileInfo fi) => fi.Delete());
    }

    /// <summary>
    ///     Replaces all instances of <see cref="Path.DirectorySeparatorChar" /> with
    ///     <see cref="Path.AltDirectorySeparatorChar" />
    /// </summary>
    /// <param name="path">path to get alt path from</param>
    public static string GetAltPath(string path) =>
        path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    /// <summary>
    ///     Replaces all instances of <see cref="Path.AltDirectorySeparatorChar" /> with
    ///     <see cref="Path.DirectorySeparatorChar" />
    /// </summary>
    /// <param name="path">path to get normal path from</param>
    public static string GetNormalPath(string path) =>
        path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

    internal static string NormalizePath(string path) => path.Replace('\\', '/');

    private static void CopyTo(string sourcePath, string destinationPath, bool overwrite = true) =>
        CopyTo(new FileInfo(sourcePath), new FileInfo(destinationPath), overwrite);

    private static void CopyTo(FileInfo source, FileInfo destination, bool overwrite = true,
        Action<long>? onWrite = null)
    {
        using FileStream srcstream = new(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
        using FileStream deststream = new(destination.FullName,
            overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read);
        CopyTo(srcstream, deststream, srcstream.Length, onWrite);
    }

    private static async Task CopyToAsync(FileInfo source, FileInfo destination, bool overwrite = true,
        Action<long>? onWrite = null)
    {
        await using FileStream srcStream = new(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read,
            DefaultFileStreamBufferSize, true);
        await using FileStream destStream = new(destination.FullName,
            overwrite
                ? FileMode.Create
                : FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            DefaultFileStreamBufferSize,
            true);
        await CopyToAsync(srcStream, destStream, srcStream.Length, onWrite);
    }

    private static async Task CopyToAsync(FileInfo source, FileInfo destination, bool overwrite = true,
        Func<long, Task>? onWrite = null)
    {
        await using FileStream srcStream = new(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read,
            DefaultFileStreamBufferSize, true);
        await using FileStream destStream = new(destination.FullName,
            overwrite
                ? FileMode.Create
                : FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            DefaultFileStreamBufferSize,
            true);
        await CopyToAsync(srcStream, destStream, srcStream.Length, onWrite);
    }

    private static void CopyTo(FileStream source, FileStream destination, long length, Action<long>? onWrite = null)
    {
        // look for enabling long here instead of int to handle larger data
        int wantedBuffersize = (int)Math.Min(Math.Pow(2, 19), length);

        ArrayPool<byte> pool = ArrayPool<byte>.Shared;
        byte[] buffer = pool.Rent(wantedBuffersize);

        try
        {
            int read;
            long totalRead = 0;
            while ((read = source.Read(buffer, 0, (int)Math.Min(wantedBuffersize, length - totalRead))) > 0 &&
                   totalRead < length)
            {
                destination.Write(buffer, 0, read);
                totalRead += read;
                onWrite?.Invoke(read);
            }
        }
        finally
        {
            pool.Return(buffer);
        }
    }

    private static async Task CopyToAsync(FileStream source, FileStream destination, long length,
        Action<long>? onWrite = null)
    {
        int wantedBuffersize = GetBufferSize(length);

        MemoryPool<byte> pool = MemoryPool<byte>.Shared;
        using IMemoryOwner<byte> owner = pool.Rent(wantedBuffersize);
        Memory<byte> buffer = owner.Memory;

        int read;
        long totalRead = 0;
        while ((read = await source.ReadAsync(buffer)) > 0 && totalRead < length)
        {
            await destination.WriteAsync(buffer.Slice(0, read));
            totalRead += read;
            onWrite?.Invoke(read);
        }
    }

    private static async Task CopyToAsync(FileStream source, FileStream destination, long length,
        Func<long, Task>? onWrite = null)
    {
        int wantedBuffersize = GetBufferSize(length);

        MemoryPool<byte> pool = MemoryPool<byte>.Shared;
        using IMemoryOwner<byte> owner = pool.Rent(wantedBuffersize);
        Memory<byte> buffer = owner.Memory;

        int read;
        long totalRead = 0;
        while ((read = await source.ReadAsync(buffer)) > 0 && totalRead < length)
        {
            await destination.WriteAsync(buffer.Slice(0, read));
            totalRead += read;
            if (onWrite is not null)
            {
                await onWrite(read);
            }
        }
    }

    private static double GetBufferSize() => Math.Pow(2, 19);
    private static int GetBufferSize(long length) => (int)Math.Min(GetBufferSize(), length);

    #region Count

    /// <summary>
    ///     Counts the number of <see cref="DirectoryInfo" /> inside the <paramref name="root" /> path.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns></returns>
    public static ulong CountFolders(string root)
    {
        ulong folderCount = 0;
        Walk(root, (DirectoryInfo _) => folderCount++);

        // folderCount has to be decreased by 1 because walk calls OnDirectory for the root dir
        return --folderCount;
    }

    /// <summary>
    ///     Counts the number of <see cref="FileInfo" /> inside the <paramref name="root" /> path.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns></returns>
    public static ulong CountFiles(string root)
    {
        ulong fileCount = 0;

        Walk(root, (FileInfo _) => fileCount++);

        return fileCount;
    }

    /// <summary>
    ///     Counts the number of <see cref="FileInfo" /> and <see cref="DirectoryInfo" /> inside the <paramref name="root" />
    ///     path.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns></returns>
    public static (ulong fileCount, ulong folderCount) CountFilesAndFolders(string root)
    {
        ulong fileCount = 0;

        ulong folderCount = 0;
        Walk(root, _ => folderCount++, _ => fileCount++);

        // folderCount has to be decreased by 1 because walk calls OnDirectory for the root dir
        return (fileCount, --folderCount);
    }

    #endregion Count

    #region Walk - Delegate

    /// <summary>
    ///     Walks a path, invoking <paramref name="onDirectory" /> on every <see cref="DirectoryInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onDirectory">
    ///     <para>
    ///         Method that takes a <see cref="DirectoryInfo" /> as parameter, used to perform operations on a
    ///         <see cref="DirectoryInfo" />.
    ///     </para>
    ///     <para>If this returns false it will skip walking this path.</para>
    /// </param>
    public static void Walk(string root, Func<DirectoryInfo, bool> onDirectory) => Walk(root, onDirectory, null);

    /// <summary>
    ///     Walks a path, invoking <paramref name="onFile" /> on every <see cref="FileInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onFile">
    ///     <para>
    ///         Method that takes a <see cref="FileInfo" /> as parameter, used to perform operations on a
    ///         <see cref="FileInfo" />.
    ///     </para>
    ///     <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
    /// </param>
    public static void Walk(string root, Func<FileInfo, bool> onFile) => Walk(root, null, onFile);

    /// <summary>
    ///     Walks a path, invoking <paramref name="onDirectory" /> on every <see cref="DirectoryInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onDirectory">
    ///     <para>
    ///         Method that takes a <see cref="DirectoryInfo" /> as parameter, used to perform operations on a
    ///         <see cref="DirectoryInfo" />.
    ///     </para>
    /// </param>
    public static void Walk(string root, Action<DirectoryInfo> onDirectory) => Walk(root, onDirectory, null);

    /// <summary>
    ///     Walks a path, invoking <paramref name="onFile" /> on every <see cref="FileInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onFile">
    ///     <para>
    ///         Method that takes a <see cref="FileInfo" /> as parameter, used to perform operations on a
    ///         <see cref="FileInfo" />.
    ///     </para>
    /// </param>
    public static void Walk(string root, Action<FileInfo> onFile) => Walk(root, null, onFile);

    /// <summary>
    ///     Walks a path, invoking <paramref name="onDirectory" /> on every <see cref="DirectoryInfo" /> and
    ///     <paramref name="onFile" /> on every <see cref="FileInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onDirectory">
    ///     <para>
    ///         Method that takes a <see cref="DirectoryInfo" /> as parameter, used to perform operations on a
    ///         <see cref="DirectoryInfo" />.
    ///     </para>
    /// </param>
    /// <param name="onFile">
    ///     <para>
    ///         Method that takes a <see cref="FileInfo" /> as parameter, used to perform operations on a
    ///         <see cref="FileInfo" />.
    ///     </para>
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Walk(string root, Action<DirectoryInfo>? onDirectory, Action<FileInfo>? onFile)
    {
        Func<DirectoryInfo, bool>? onDirFunc = null;
        if (onDirectory is { })
        {
            onDirFunc = di =>
            {
                onDirectory.Invoke(di);
                return true;
            };
        }

        Func<FileInfo, bool>? onFileFunc = null;
        if (onFile is { })
        {
            onFileFunc = fi =>
            {
                onFile.Invoke(fi);
                return true;
            };
        }

        Walk(root, onDirFunc, onFileFunc);
    }

    /// <summary>
    ///     Walks a path, invoking <paramref name="onDirectory" /> on every Directory and <paramref name="onFile" /> on every
    ///     File found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onDirectory">
    ///     <para>Method that takes a <see cref="string" /> as parameter, used to perform operations on a Directory.</para>
    ///     <para>If this returns false it will skip walking this path.</para>
    /// </param>
    /// <param name="onFile">
    ///     <para>Method that takes a <see cref="string" /> as parameter, used to perform operations on a File.</para>
    ///     <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
    /// </param>
    public static void Walk(string root, Func<string, bool>? onDirectory, Func<string, bool>? onFile)
    {
        // Data structure to hold names of subfolders to be
        // examined for files.
        Stack<string> dirs = new(InitialCapacity);
        if (!Directory.Exists(root))
        {
            throw new ArgumentException($"Directory does not exist. '{root}'", nameof(root));
        }

        bool checkFiles = onFile is { };

        dirs.Push(NormalizePath(root));
        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();

            if (onDirectory?.Invoke(currentDir) != false)
            {
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable
                // to ignore the exception and continue enumerating the remaining files and
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The
                // choice of which exceptions to catch depends entirely on the specific task
                // you are intending to perform and also on how much you know with certainty
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                subDirs.Call(NormalizePath).Call(dirs.Push);

                // only go through files if we have a file handler
                if (checkFiles)
                {
                    string[] files;
                    try
                    {
                        files = Directory.GetFiles(currentDir);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }

                    // Perform the required action on each file here.
                    foreach (string file in files.Call(NormalizePath))
                    {
                        try
                        {
                            // this is definitely not null due to onFile is {} check and we're in true branch
                            if (!onFile!.Invoke(file))
                            {
                                break;
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            // If file was deleted by a separate application
                            // or thread since the call to TraverseTree()
                            // then just continue.
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Walks a path, invoking <paramref name="onDirectory" /> on every <see cref="DirectoryInfo" /> and
    ///     <paramref name="onFile" /> on every <see cref="FileInfo" /> found.
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="onDirectory">
    ///     <para>
    ///         Method that takes a <see cref="DirectoryInfo" /> as parameter, used to perform operations on a
    ///         <see cref="DirectoryInfo" />.
    ///     </para>
    ///     <para>If this returns false it will skip walking this path.</para>
    /// </param>
    /// <param name="onFile">
    ///     <para>
    ///         Method that takes a <see cref="FileInfo" /> as parameter, used to perform operations on a
    ///         <see cref="FileInfo" />.
    ///     </para>
    ///     <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
    /// </param>
    public static void Walk(string root, Func<DirectoryInfo, bool>? onDirectory, Func<FileInfo, bool>? onFile)
    {
        // Data structure to hold names of subfolders to be
        // examined for files.
        Stack<string> dirs = new(InitialCapacity);
        if (!Directory.Exists(root))
        {
            throw new ArgumentException($"Directory does not exist. '{root}'", nameof(root));
        }

        bool checkFiles = onFile is { };

        dirs.Push(NormalizePath(root));
        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();

            DirectoryInfo di = new(currentDir);
            if (onDirectory?.Invoke(di) != false)
            {
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable
                // to ignore the exception and continue enumerating the remaining files and
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The
                // choice of which exceptions to catch depends entirely on the specific task
                // you are intending to perform and also on how much you know with certainty
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                subDirs.Call(NormalizePath).Call(dirs.Push);

                // only go through files if we have a file handler
                if (checkFiles)
                {
                    string[] files;
                    try
                    {
                        files = Directory.GetFiles(currentDir);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }

                    // Perform the required action on each file here.
                    foreach (string file in files.Call(NormalizePath))
                    {
                        try
                        {
                            FileInfo fi = new(file);
                            // this is definitely not null due to onFile is {} check and we're in true branch
                            if (!onFile!.Invoke(fi))
                            {
                                break;
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            // If file was deleted by a separate application
                            // or thread since the call to TraverseTree()
                            // then just continue.
                        }
                    }
                }
            }
        }
    }

    #endregion Walk - Delegate

    #region Walk - Enumerable

    /// <summary>
    ///     Enumerates the tree of a root path, returning files and directories
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <returns></returns>
    public static IEnumerable<string> Walk(string root) => Walk(root, FileSystemEnumeration.FilesAndDirectories);

    /// <summary>
    ///     Enumerates the tree of a root path
    /// </summary>
    /// <param name="root">Path where to start walking</param>
    /// <param name="fileSystemEnumeration">Whether to return only files, directories or both</param>
    /// <returns></returns>
    public static IEnumerable<string> Walk(string root, FileSystemEnumeration fileSystemEnumeration)
    {
        // Data structure to hold names of subfolders to be
        // examined for files.
        Stack<string> dirs = new(InitialCapacity);
        if (!Directory.Exists(root))
        {
            throw new ArgumentException($"Directory does not exist. '{root}'", nameof(root));
        }

        bool enumFiles =
            fileSystemEnumeration is FileSystemEnumeration.FilesOnly or FileSystemEnumeration.FilesAndDirectories;
        bool enumDirs =
            fileSystemEnumeration is FileSystemEnumeration.DirectoriesOnly or FileSystemEnumeration
                .FilesAndDirectories;

        dirs.Push(NormalizePath(root));
        while (dirs.Count > 0)
        {
            string currentDir = dirs.Pop();

            if (enumDirs)
            {
                yield return currentDir;
            }

            IEnumerable<string> subDirs;
            try
            {
                subDirs = Directory.EnumerateDirectories(currentDir);
            }
            // An UnauthorizedAccessException exception will be thrown if we do not have
            // discovery permission on a folder or file. It may or may not be acceptable
            // to ignore the exception and continue enumerating the remaining files and
            // folders. It is also possible (but unlikely) that a DirectoryNotFound exception
            // will be raised. This will happen if currentDir has been deleted by
            // another application or thread after our call to Directory.Exists. The
            // choice of which exceptions to catch depends entirely on the specific task
            // you are intending to perform and also on how much you know with certainty
            // about the systems on which this code will run.
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (DirectoryNotFoundException)
            {
                continue;
            }

            // Push the subdirectories onto the stack for traversal.
            // This could also be done before handing the files.
            subDirs.Call(NormalizePath).Call(dirs.Push);

            // only go through files if we have a file handler
            if (enumFiles)
            {
                IEnumerable<string> files;
                try
                {
                    files = Directory.EnumerateFiles(currentDir);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                foreach (string file in files.Call(NormalizePath))
                {
                    yield return file;
                }
            }
        }
    }

    #endregion Walk - Enumerable
}
