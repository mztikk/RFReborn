﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RFReborn.AoB;
using RFReborn.Comparison;

namespace RFReborn.Files
{
    /// <summary>
    /// Utilities to interact with the FileSystem
    /// </summary>
    public static class FileUtils
    {
        private const int InitialCapacity = 64;

        /// <summary>
        /// Checks if there is any File inside a root path
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns>Returns <see langword="true"/> if there is any File; <see langword="false"/> otherwise.</returns>
        public static bool AnyFile(string root)
        {
            Stack<string> dirs = new Stack<string>(InitialCapacity);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
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
#pragma warning disable CA1031 // Do not catch general exception types
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types

                if (files.Length > 0)
                {
                    return true;
                }

                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types

                foreach (string str in subDirs)
                {
                    dirs.Push(str);
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the number of <see cref="DirectoryInfo"/> inside the <paramref name="root"/> path.
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
        /// Counts the number of <see cref="FileInfo"/> inside the <paramref name="root"/> path.
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
        /// Counts the number of <see cref="FileInfo"/> and <see cref="DirectoryInfo"/> inside the <paramref name="root"/> path.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns></returns>
        public static (ulong fileCount, ulong folderCount) CountFilesAndFolders(string root)
        {
            ulong fileCount = 0;

            ulong folderCount = 0;
            Walk(root, (DirectoryInfo _) => folderCount++, (FileInfo _) => fileCount++);

            // folderCount has to be decreased by 1 because walk calls OnDirectory for the root dir
            return (fileCount, --folderCount);
        }

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">
        /// <para>Method that takes a <see cref="DirectoryInfo"/> as parameter, used to perform operations on a <see cref="DirectoryInfo"/>.</para>
        /// <para>If this returns false it will skip walking this path.</para>
        /// </param>
        public static void Walk(string root, Func<DirectoryInfo, bool> onDirectory) => Walk(root, onDirectory, null);

        /// <summary>
        /// Walks a path, invoking <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onFile">
        /// <para>Method that takes a <see cref="FileInfo"/> as parameter, used to perform operations on a <see cref="FileInfo"/>.</para>
        /// <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
        /// </param>
        public static void Walk(string root, Func<FileInfo, bool> onFile) => Walk(root, null, onFile);

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">
        /// <para>Method that takes a <see cref="DirectoryInfo"/> as parameter, used to perform operations on a <see cref="DirectoryInfo"/>.</para>
        /// </param>
        public static void Walk(string root, Action<DirectoryInfo> onDirectory) => Walk(root, onDirectory, null);

        /// <summary>
        /// Walks a path, invoking <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onFile">
        /// <para>Method that takes a <see cref="FileInfo"/> as parameter, used to perform operations on a <see cref="FileInfo"/>.</para>
        /// </param>
        public static void Walk(string root, Action<FileInfo> onFile) => Walk(root, null, onFile);

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> and <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">
        /// <para>Method that takes a <see cref="DirectoryInfo"/> as parameter, used to perform operations on a <see cref="DirectoryInfo"/>.</para>
        /// </param>
        /// <param name="onFile">
        /// <para>Method that takes a <see cref="FileInfo"/> as parameter, used to perform operations on a <see cref="FileInfo"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Walk(string root, Action<DirectoryInfo>? onDirectory, Action<FileInfo>? onFile)
        {
            Func<DirectoryInfo, bool>? onDirFunc = null;
            if (onDirectory is { })
            {
                onDirFunc = (DirectoryInfo di) =>
                {
                    onDirectory.Invoke(di);
                    return true;
                };
            }

            Func<FileInfo, bool>? onFileFunc = null;
            if (onFile is { })
            {
                onFileFunc = (FileInfo fi) =>
                {
                    onFile.Invoke(fi);
                    return true;
                };
            }

            Walk(root, onDirFunc, onFileFunc);
        }

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> and <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">
        /// <para>Method that takes a <see cref="DirectoryInfo"/> as parameter, used to perform operations on a <see cref="DirectoryInfo"/>.</para>
        /// <para>If this returns false it will skip walking this path.</para>
        /// </param>
        /// <param name="onFile">
        /// <para>Method that takes a <see cref="FileInfo"/> as parameter, used to perform operations on a <see cref="FileInfo"/>.</para>
        /// <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
        /// </param>
        public static void Walk(string root, Func<DirectoryInfo, bool>? onDirectory, Func<FileInfo, bool>? onFile)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(InitialCapacity);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            bool checkFiles = onFile is { };

            dirs.Push(root);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                DirectoryInfo di = new DirectoryInfo(currentDir);
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
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    // Push the subdirectories onto the stack for traversal.
                    // This could also be done before handing the files.
                    foreach (string str in subDirs)
                    {
                        dirs.Push(str);
                    }

                    // only go through files if we have a file handler
                    if (checkFiles)
                    {
                        string[] files;
                        try
                        {
                            files = Directory.GetFiles(currentDir);
                        }
#pragma warning disable CA1031 // Do not catch general exception types
                        catch (UnauthorizedAccessException)
                        {
                            continue;
                        }
                        catch (DirectoryNotFoundException)
                        {
                            continue;
                        }
#pragma warning restore CA1031 // Do not catch general exception types

                        // Perform the required action on each file here.
                        foreach (string file in files)
                        {
                            try
                            {
                                FileInfo fi = new FileInfo(file);
                                // this is definitely not null due to onFile is {} check and we're in true branch
                                if (!onFile!.Invoke(fi))
                                {
                                    break;
                                }
                            }
#pragma warning disable CA1031 // Do not catch general exception types
                            catch (FileNotFoundException)
                            {
                                // If file was deleted by a separate application
                                // or thread since the call to TraverseTree()
                                // then just continue.
                                continue;
                            }
#pragma warning restore CA1031 // Do not catch general exception types
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates the tree of a root path, returning files and directories
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns></returns>
        public static IEnumerable<string> Walk(string root) => Walk(root, FileSystemEnumeration.FilesAndDirectories);

        /// <summary>
        /// Enumerates the tree of a root path
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="fileSystemEnumeration">Whether to return only files, directories or both</param>
        /// <returns></returns>
        public static IEnumerable<string> Walk(string root, FileSystemEnumeration fileSystemEnumeration)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(InitialCapacity);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            bool enumFiles = fileSystemEnumeration == FileSystemEnumeration.FilesOnly || fileSystemEnumeration == FileSystemEnumeration.FilesAndDirectories;
            bool enumDirs = fileSystemEnumeration == FileSystemEnumeration.DirectoriesOnly || fileSystemEnumeration == FileSystemEnumeration.FilesAndDirectories;

            dirs.Push(root);
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
#pragma warning disable CA1031 // Do not catch general exception types
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }
#pragma warning restore CA1031 // Do not catch general exception types

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                {
                    dirs.Push(str);
                }

                // only go through files if we have a file handler
                if (enumFiles)
                {
                    IEnumerable<string> files;
                    try
                    {
                        files = Directory.EnumerateFiles(currentDir);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }
#pragma warning restore CA1031 // Do not catch general exception types

                    foreach (string file in files)
                    {
                        yield return file;
                    }
                }
            }
        }

        /// <summary>
        /// Finds files by walking the root and wildcard matching the path and alt path with a given mask
        /// </summary>
        /// <param name="root">Root path to start walking</param>
        /// <param name="pattern">Wildcard mask to match filepath</param>
        public static IEnumerable<string> FindFilesByMatch(string root, string pattern)
        {
            foreach (string file in Walk(root, FileSystemEnumeration.FilesOnly))
            {
                string[] tests = new string[] { Path.GetFileName(file), file, GetAltPath(file) };
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
        /// Finds files by walking the root and matching the <see cref="Signature"/>
        /// </summary>
        /// <param name="root">Root path to start walking</param>
        /// <param name="signature"><see cref="Signature"/> to match</param>
        /// <param name="continueOnException">If <see langword="true"/> continues on exception, if <see langword="false"/> throws them. May happen when reading from files</param>
        public static IEnumerable<string> FindFilesBySignature(string root, Signature signature, bool continueOnException = true)
        {
            foreach (string file in Walk(root, FileSystemEnumeration.FilesOnly))
            {
                bool flag = false;
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (Scanner.FindSignature(fs, signature) != -1)
                        {
                            flag = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (continueOnException)
                    {
                        continue;
                    }
                    else
                    {
                        throw e;
                    }
                }

                if (flag)
                {
                    yield return file;
                }
            }
        }

        /// <summary>
        /// Copies all files and directories from <paramref name="source"/> to <paramref name="destination"/>
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
                (FileInfo fi) =>
                {
                    string relPath = Path.GetRelativePath(source.FullName, fi.FullName);
                    string newPath = Path.Combine(destination.FullName, relPath);
                    FileInfo newFile = new FileInfo(newPath);
                    if (!newFile.Directory.Exists)
                    {
                        newFile.Directory.Create();
                    }
                    fi.CopyTo(newPath, false);
                });
        }

        /// <summary>
        /// Copies files from <paramref name="source"/> <see cref="DirectoryInfo"/> to <paramref name="target"/> <see cref="DirectoryInfo"/>
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

            IEnumerable<string> files = onlyDiffFiles ? GetDiffFiles(source, target) : Walk(source.FullName, FileSystemEnumeration.FilesOnly);
            foreach (string file in files)
            {
                string filePath = Path.Combine(source.FullName, file);
                string newFilePath = Path.Combine(target.FullName, file);
                if (!File.Exists(newFilePath))
                {
                    Directory.CreateDirectory(Directory.GetParent(newFilePath).FullName);
                    File.Create(newFilePath).Dispose();
                }

                File.Copy(filePath, newFilePath);
                yield return file;
            }
        }

        /// <summary>
        /// Returns the relative path of all files which are different between the source and target <see cref="DirectoryInfo"/>
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
        /// Compares two files for inequality
        /// </summary>
        /// <param name="left">First file to compare</param>
        /// <param name="right">Second file to compare</param>
        /// <returns><see langword="true"/> if files are different, <see langword="false"/> otherwise</returns>
        public static bool AreDifferent(this FileInfo left, FileInfo right)
        {
            using (FileStream leftData = left.OpenRead())
            {
                using (FileStream rightData = right.OpenRead())
                {
                    return FastCompare.NotEquals(leftData, rightData);
                }
            }
        }

        /// <summary>
        /// Compares two files for inequality
        /// </summary>
        /// <param name="left">First file to compare</param>
        /// <param name="right">Second file to compare</param>
        /// <returns><see langword="true"/> if files are different, <see langword="false"/> otherwise</returns>
        public static bool AreDifferent(string left, string right)
        {
            using (FileStream leftData = File.OpenRead(left))
            {
                using (FileStream rightData = File.OpenRead(right))
                {
                    return FastCompare.NotEquals(leftData, rightData);
                }
            }
        }

        /// <summary>
        /// Clears a <see cref="DirectoryInfo"/>, deleting all files inside
        /// </summary>
        /// <param name="directoryInfo"><see cref="DirectoryInfo"/> to clear</param>
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
        /// Replaces all instances of <see cref="Path.DirectorySeparatorChar"/> with <see cref="Path.AltDirectorySeparatorChar"/>
        /// </summary>
        /// <param name="path">path to get alt path from</param>
        public static string GetAltPath(string path) => path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
}
