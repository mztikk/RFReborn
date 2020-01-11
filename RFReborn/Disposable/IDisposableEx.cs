using System;

namespace RFReborn.Disposable
{
    /// <summary>
    /// Provides a mechanism for releasing unmanaged resources with additional info.
    /// </summary>
    public interface IDisposableEx : IDisposable
    {
        /// <summary>
        /// If the object has been disposed
        /// </summary>
        public bool IsDisposed { get; }
    }
}
