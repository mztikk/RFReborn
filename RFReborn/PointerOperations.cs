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
            (*start, *end) = (*end, *start);
            start++;
            end--;
        }
    }
}
