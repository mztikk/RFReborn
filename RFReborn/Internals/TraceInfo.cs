using System;
using System.Diagnostics;

namespace RFReborn.Internals;

/// <summary>
/// Provides methods to access trace and stack information
/// </summary>
public static class TraceInfo
{
    /// <summary>
    /// Walks the <see cref="StackTrace"/> starting at <paramref name="skipFrames"/> until <paramref name="check"/> returns <see langword="true"/> for the current <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="check">Method delegate to use for checking against the current <see cref="StackFrame"/></param>
    /// <param name="skipFrames">Number of frames from which to start walking</param>
    /// <returns>The <see cref="StackFrame"/> that matched the <paramref name="check"/> or <see langword="null"/> if nothing was found.</returns>
    public static StackFrame? WalkStackTrace(Func<StackFrame, bool> check, int skipFrames)
    {
        StackTrace stackTrace = new(skipFrames);
        foreach (StackFrame frame in stackTrace.GetFrames())
        {
            if (check(frame))
            {
                return frame;
            }
        }

        return null;
    }

    /// <summary>
    /// Walks the <see cref="StackTrace"/> starting at default frame value of 1 until <paramref name="check"/> returns <see langword="true"/> for the current <see cref="StackFrame"/>.
    /// </summary>
    /// <param name="check">Method delegate to use for checking against the current <see cref="StackFrame"/></param>
    /// <returns>The <see cref="StackFrame"/> that matched the <paramref name="check"/> or <see langword="null"/> if nothing was found.</returns>
    public static StackFrame? WalkStackTrace(Func<StackFrame, bool> check) => WalkStackTrace(check, 1);
}
