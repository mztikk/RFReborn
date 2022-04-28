using System;

namespace RFReborn.Disposable;

/// <summary>
/// Wrapper around <see cref="IDisposable"/> which calls a <see cref="Action"/> on being disposed
/// </summary>
public class DisposableAction : IDisposable
{
    private readonly Action _action;
    private bool _disposedValue;

    /// <summary>
    /// Constructs a new <see cref="DisposableAction"/>
    /// </summary>
    /// <param name="action">Action to call on being disposed</param>
    public DisposableAction(Action action) => _action = action;

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _action();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DisposableAction()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    /// <inheritdoc />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
