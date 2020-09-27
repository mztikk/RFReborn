using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using RFReborn.AoB;
using RFReborn.Comparison;
using RFReborn.Extensions;

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

        #region Count
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
        #endregion Count

        #region Walk - Delegate
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
        /// Walks a path, invoking <paramref name="onDirectory"/> on every Directory and <paramref name="onFile"/> on every File found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">
        /// <para>Method that takes a <see cref="string"/> as parameter, used to perform operations on a Directory.</para>
        /// <para>If this returns false it will skip walking this path.</para>
        /// </param>
        /// <param name="onFile">
        /// <para>Method that takes a <see cref="string"/> as parameter, used to perform operations on a File.</para>
        /// <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
        /// </param>
        public static void Walk(string root, Func<string, bool>? onDirectory, Func<string, bool>? onFile)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(InitialCapacity);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
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
                    foreach (string str in subDirs.Call(NormalizePath))
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

            dirs.Push(NormalizePath(root));
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
                    foreach (string str in subDirs.Call(NormalizePath))
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
                        foreach (string file in files.Call(NormalizePath))
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
        #endregion Walk - Delegate

        #region Walk - Enumerable
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
                foreach (string str in subDirs.Call(NormalizePath))
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

                    foreach (string file in files.Call(NormalizePath))
                    {
                        yield return file;
                    }
                }
            }
        }
        #endregion Walk - Enumerable

        #region GetFiles
        /// <summary>
        /// Enumerates the tree of a root path and yields all files
        /// </summary>
        /// <param name="root">Path to start walking</param>
        /// <param name="skipDirectory"><see cref="Func{T, TResult}"/> to check each <see cref="DirectoryInfo"/>; if this returns true it will be skipped and not further traversed</param>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(string root, Func<DirectoryInfo, bool>? skipDirectory)
        {
            Stack<string> dirs = new Stack<string>(InitialCapacity);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            dirs.Push(NormalizePath(root));
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                DirectoryInfo di = new DirectoryInfo(currentDir);
                if (skipDirectory?.Invoke(di) == false)
                {
                    IEnumerable<string> subDirs;
                    try
                    {
                        subDirs = Directory.EnumerateDirectories(currentDir);
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

                    foreach (string str in subDirs.Call(NormalizePath))
                    {
                        dirs.Push(str);
                    }

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

                    foreach (string file in files.Call(NormalizePath))
                    {
                        yield return file;
                    }
                }
            }
        }
        #endregion GetFiles

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
                bool findFlag = false;
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        if (Scanner.FindSignature(fs, signature) != -1)
                        {
                            findFlag = true;
                        }
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
        /// Copies all files from <paramref name="files"/> based on <paramref name="baseOrigin"/> <see cref="DirectoryInfo"/> relative path to <paramref name="destination"/>
        /// </summary>
        /// <param name="baseOrigin">Base Origin for relative path</param>
        /// <param name="destination">Destination to copy to</param>
        /// <param name="files">Files to copy</param>
        /// <param name="overwrite">Overwrite files</param>
        public static void Copy(DirectoryInfo baseOrigin, DirectoryInfo destination, IEnumerable<FileInfo> files, bool overwrite = true)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            foreach (FileInfo file in files)
            {
                if (!file.Exists)
                {
                    throw new ArgumentException();
                }

                string relPath = Path.GetRelativePath(baseOrigin.FullName, file.FullName);
                string newPath = Path.Combine(destination.FullName, relPath);
                FileInfo newFile = new FileInfo(newPath);
                if (!newFile.Directory.Exists)
                {
                    newFile.Directory.Create();
                }

                CopyTo(file, newFile, overwrite);
                //file.CopyTo(newPath, overwrite);
            }
        }

        /// <summary>
        /// Makes path of a file to a new base destination relative from a base origin
        /// <para/>
        /// For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file would translate to D:\newdir\diffsub\subdir\abc\bin.exe
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
        /// Makes path of a file to a new base destination relative from a base origin
        /// <para/>
        /// For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file would translate to D:\newdir\diffsub\subdir\abc\bin.exe
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
        /// Makes path of a file to a new base destination relative from a base origin
        /// <para/>
        /// For example: C:\testdir as origin and D:\newdir\diffsub\ as destination and C:\testdir\subdir\abc\bin.exe as file would translate to D:\newdir\diffsub\subdir\abc\bin.exe
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
        /// Copies a file from <paramref name="file"/> based on <paramref name="baseOrigin"/> <see cref="DirectoryInfo"/> relative path to <paramref name="destination"/>
        /// </summary>
        /// <param name="baseOrigin">Base Origin for relative path</param>
        /// <param name="destination">Destination to copy to</param>
        /// <param name="file">File to copy</param>
        /// <param name="overwrite">Overwrite files</param>
        public static void Copy(DirectoryInfo baseOrigin, DirectoryInfo destination, FileInfo file, bool overwrite = true)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            if (!file.Exists)
            {
                throw new ArgumentException();
            }

            string relPath = Path.GetRelativePath(baseOrigin.FullName, file.FullName);
            string newPath = Path.Combine(destination.FullName, relPath);
            FileInfo newFile = new FileInfo(newPath);
            if (!newFile.Directory.Exists)
            {
                newFile.Directory.Create();
            }

            CopyTo(file, newFile, overwrite);
            //file.CopyTo(newPath, overwrite);
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
                    CopyTo(fi, newFile, false);
                    //fi.CopyTo(newPath, false);
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

                //File.Copy(filePath, newFilePath);
                CopyTo(filePath, newFilePath, false);
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
        /// Copies a file to a destination
        /// </summary>
        /// <param name="source">Source file to copy</param>
        /// <param name="destination">Destination to copy to</param>
        /// <param name="overwrite">Allow overwriting of destination file or not</param>
        /// <param name="onWrite">Action will be called everytime theres an advance in bytes with the number of bytes as parameter, to track progress. Can be null.</param>
        /// <exception cref="IOException">If the destination already exists and overwriting is not allowed</exception>
        public static void Copy(FileInfo source, FileInfo destination, bool overwrite = true, Action<long>? onWrite = null) => CopyTo(source, destination, overwrite, onWrite);

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
        /// Compares two files for equality
        /// </summary>
        /// <param name="left">First file to compare</param>
        /// <param name="right">Second file to compare</param>
        /// <returns><see langword="true"/> if files are equal, <see langword="false"/> otherwise</returns>
        public static bool AreEqual(this FileInfo left, FileInfo right)
        {
            using (FileStream leftData = left.OpenRead())
            {
                using (FileStream rightData = right.OpenRead())
                {
                    return FastCompare.Equals(leftData, rightData);
                }
            }
        }

        /// <summary>
        /// Compares two files for equality
        /// </summary>
        /// <param name="left">First file to compare</param>
        /// <param name="right">Second file to compare</param>
        /// <returns><see langword="true"/> if files are equal, <see langword="false"/> otherwise</returns>
        public static bool AreEqual(string left, string right)
        {
            using (FileStream leftData = File.OpenRead(left))
            {
                using (FileStream rightData = File.OpenRead(right))
                {
                    return FastCompare.Equals(leftData, rightData);
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

        /// <summary>
        /// Replaces all instances of <see cref="Path.AltDirectorySeparatorChar"/> with <see cref="Path.DirectorySeparatorChar"/>
        /// </summary>
        /// <param name="path">path to get normal path from</param>
        public static string GetNormalPath(string path) => path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        internal static string NormalizePath(string path) => path.Replace('\\', '/');

        private static void CopyTo(string sourcePath, string destinationPath, bool overwrite = true) => CopyTo(new FileInfo(sourcePath), new FileInfo(destinationPath), overwrite);

        private static void CopyTo(FileInfo source, FileInfo destination, bool overwrite = true, Action<long>? onWrite = null)
        {
            using var srcstream = new FileStream(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var deststream = new FileStream(destination.FullName, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            CopyTo(srcstream, deststream, srcstream.Length, onWrite);
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
                while ((read = source.Read(buffer, 0, (int)Math.Min(wantedBuffersize, length - totalRead))) > 0 && totalRead < length)
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
    }
}
