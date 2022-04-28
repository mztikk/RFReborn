using System;
using System.Diagnostics.CodeAnalysis;

namespace RFReborn.Files;

/// <summary>
/// Creates a temporary file from content which gets deleted when disposing
/// </summary>
public class TempFile : IDisposable
{
    private bool _disposedValue;

    /// <summary>
    /// Temporary file
    /// </summary>
    public FileInfo File { get; private set; }

    /// <summary>
    /// Constructs the temporary file from a stream
    /// </summary>
    /// <param name="content">Content the temp file holds</param>
    // Not null since CreateFile call
    public TempFile(Stream content)
    {
        CreateFile();

        using FileStream stream = File.OpenWrite();
        content.CopyTo(stream);
    }

    /// <summary>
    /// Constructs the temporary file from a string
    /// </summary>
    /// <param name="content">Content the temp file holds</param>
    // Not null since CreateFile call
    public TempFile(string content)
    {
        CreateFile();

        using FileStream stream = File.OpenWrite();
        using StreamWriter writer = new(stream);
        writer.WriteLine(content);
    }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    // Private constructor for static create methods
    private TempFile() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

    [MemberNotNull(nameof(File))]
    private void CreateFile() => File = new FileInfo(Path.GetRandomFileName());

    /// <summary>
    /// Asynchronously creates the temp file
    /// </summary>
    /// <param name="content">Content the temp file holds</param>
    public static async Task<TempFile> Create(Stream content)
    {
        TempFile tempFile = new();
        tempFile.CreateFile();

        using (FileStream stream = tempFile.File.OpenWrite())
        {
            await content.CopyToAsync(stream);
        }

        return tempFile;
    }

    /// <summary>
    /// Asynchronously creates the temp file
    /// </summary>
    /// <param name="content">Content the temp file holds</param>
    /// <returns></returns>
    public static async Task<TempFile> Create(string content)
    {
        TempFile tempFile = new();
        tempFile.CreateFile();

        using FileStream stream = tempFile.File.OpenWrite();
        using StreamWriter writer = new(stream);
        await writer.WriteLineAsync(content);

        return tempFile;
    }

    /// <summary>
    /// Disposes temporary file
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                File.Delete();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TempFile()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    /// <summary>
    /// Disposes temporary file
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
