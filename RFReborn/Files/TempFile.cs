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
        public TempFile(Stream content)
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
        public TempFile(string content)
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

        private TempFile() { }

        private void CreateFile()
        {
            string name = Guid.NewGuid().ToString();
            File = new FileInfo(name);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    File.Delete();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TempFile()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() =>
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);// TODO: uncomment the following line if the finalizer is overridden above.// GC.SuppressFinalize(this);
        #endregion

        /// <summary>
        /// Asynchronously creates the temp file
        /// </summary>
        /// <param name="content">Content the temp file holds</param>
        /// <returns></returns>
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
