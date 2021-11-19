using System.Runtime.CompilerServices;

namespace RFReborn;

internal static class PointerOperations
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void Reverse<T>(T* ptr, int len) where T : unmanaged
    {
        T* start = ptr;
        T* end = start + len - 1;
        while (start < end)
        {
            T tmp = *end;
            *end = *start;
            *start = tmp;
            start++;
            end--;
        }
    }
}
