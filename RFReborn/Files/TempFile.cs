using System;
using System.IO;
using System.Threading.Tasks;

namespace RFReborn.Files
{
    /// <summary>
    /// Creates a temporary file from content which gets deleted when disposing
    /// </summary>
    public class TempFile : IDisposable
    {
        /// <summary>
        /// Temporary file
        /// </summary>
        public FileInfo File { get; private set; }

        /// <summary>
        /// Constructs the temporary file from a stream
        /// </summary>
        /// <param name="content">Content the temp file holds</param>
// Not null since CreateFile call
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public TempFile(Stream content)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            CreateFile();

            using (FileStream stream = File.OpenWrite())
            {
                content.CopyTo(stream);
            }
        }

        /// <summary>
        /// Constructs the temporary file from a string
        /// </summary>
        /// <param name="content">Content the temp file holds</param>
// Not null since CreateFile call
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public TempFile(string content)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
            CreateFile();

            using (FileStream stream = File.OpenWrite())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(content);
                }
            }
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        // Private constructor for static create methods
        private TempFile() { }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        private void CreateFile()
        {
            string name = Guid.NewGuid().ToString();
            File = new FileInfo(name);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

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
                    File.Delete();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TempFile()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Disposes temporary file
        /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly; Wrong warning because there is no block body
        public void Dispose() =>
#pragma warning restore CA1063 // Implement IDisposable Correctly
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
        #endregion

        /// <summary>
        /// Asynchronously creates the temp file
        /// </summary>
        /// <param name="content">Content the temp file holds</param>
        public static async Task<TempFile> Create(Stream content)
        {
            TempFile tempFile = new TempFile();
            tempFile.CreateFile();

            using (FileStream stream = tempFile.File.OpenWrite())
            {
                await content.CopyToAsync(stream).ConfigureAwait(false);
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
            TempFile tempFile = new TempFile();
            tempFile.CreateFile();

            using (FileStream stream = tempFile.File.OpenWrite())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    await writer.WriteLineAsync(content).ConfigureAwait(false);
                }
            }

            return tempFile;
        }
    }
}
