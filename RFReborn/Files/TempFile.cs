using System;
using System.IO;

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
        public FileInfo File { get; }

        /// <summary>
        /// Constructs the temporary file from a stream
        /// </summary>
        /// <param name="content"></param>
        public TempFile(Stream content)
        {
            string name = Guid.NewGuid().ToString();
            File = new FileInfo(name);
            using (FileStream stream = File.OpenWrite())
            {
                content.CopyTo(stream);
            }
        }

        /// <summary>
        /// Constructs the temporary file from a string
        /// </summary>
        /// <param name="content"></param>
        public TempFile(string content)
        {
            string name = Guid.NewGuid().ToString();
            File = new FileInfo(name);
            using (FileStream stream = File.OpenWrite())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(content);
                }
            }
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
    }
}
