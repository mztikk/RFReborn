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

    /// <summary>
    /// Calls the <see cref="Action"/>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _action();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Calls the <see cref="Action"/>
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
