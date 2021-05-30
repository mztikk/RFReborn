using System;
using System.Buffers;

namespace RFReborn.Disposable
{
    internal class DisposableArray<T> : IDisposable
    {
        private static readonly ArrayPool<T> s_arrayPool = ArrayPool<T>.Shared;
        private readonly DisposableAction _disposableAction;

        public readonly T[] Array;
        public readonly int MinimumLength;

        public DisposableArray(int minimumLength)
        {
            MinimumLength = minimumLength;
            Array = s_arrayPool.Rent(MinimumLength);

            _disposableAction = new DisposableAction(ReturnArray);
        }

        public void Dispose() => _disposableAction.Dispose();

        private void ReturnArray() => s_arrayPool.Return(Array);
    }
}
