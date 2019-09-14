using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RFReborn.Files
{
    /// <summary>
    /// Utilities to interact with the FileSystem
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// <para>Method that takes a <see cref="DirectoryInfo"/> as parameter, used to perform operations on a <see cref="DirectoryInfo"/>.</para>
        /// <para>If this returns false it will skip walking this path.</para>
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> to perform operations on</param>
        public delegate bool OnDirectory(DirectoryInfo directory);

        /// <summary>
        /// <para>Method that takes a <see cref="FileInfo"/> as parameter, used to perform operations on a <see cref="FileInfo"/>.</para>
        /// <para>If this returns false it will skip evaluating the rest of the files in the current directory.</para>
        /// </summary>
        /// <param name="file"><see cref="FileInfo"/> to perform operations on</param>
        public delegate bool OnFile(FileInfo file);

        /// <summary>
        /// Counts the number of <see cref="DirectoryInfo"/> inside the <paramref name="root"/> path.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <returns></returns>
        public static ulong CountFolders(string root)
        {
            ulong folderCount = 0;
            Walk(root, (DirectoryInfo _) => { folderCount++; return true; });

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

            Walk(root, (FileInfo _) => { fileCount++; return true; });

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
            Walk(root, (DirectoryInfo _) => { folderCount++; return true; }, (FileInfo _) => { fileCount++; return true; });

            // folderCount has to be decreased by 1 because walk calls OnDirectory for the root dir
            return (fileCount, --folderCount);
        }

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">Action to perform on every <see cref="DirectoryInfo"/></param>
        public static void Walk(string root, OnDirectory onDirectory) => Walk(root, onDirectory, null);

        /// <summary>
        /// Walks a path, invoking <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onFile">Action to perform on every <see cref="FileInfo"/></param>
        public static void Walk(string root, OnFile onFile) => Walk(root, null, onFile);

        /// <summary>
        /// Walks a path, invoking <paramref name="onDirectory"/> on every <see cref="DirectoryInfo"/> and <paramref name="onFile"/> on every <see cref="FileInfo"/> found.
        /// </summary>
        /// <param name="root">Path where to start walking</param>
        /// <param name="onDirectory">Action to perform on every <see cref="DirectoryInfo"/></param>
        /// <param name="onFile">Action to perform on every <see cref="FileInfo"/></param>
        public static void Walk(string root, OnDirectory onDirectory, OnFile onFile)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(20);
            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }

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
                    catch (UnauthorizedAccessException e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }

                    // Push the subdirectories onto the stack for traversal.
                    // This could also be done before handing the files.
                    foreach (string str in subDirs)
                    {
                        dirs.Push(str);
                    }

                    string[] files = null;
                    try
                    {
                        files = Directory.GetFiles(currentDir);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Debug.WriteLine(e.Message);
                        continue;
                    }

                    // Perform the required action on each file here.
                    foreach (string file in files)
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(file);
                            onFile?.Invoke(fi);
                        }
                        catch (FileNotFoundException e)
                        {
                            // If file was deleted by a separate application
                            // or thread since the call to TraverseTree()
                            // then just continue.
                            Debug.WriteLine(e.Message);
                            continue;
                        }
                    }
                }
            }
        }
    }
}
