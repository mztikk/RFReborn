using System.Diagnostics;

namespace RFReborn.Internals
{
    public static class TraceInfo
    {
        public delegate bool CheckStackFrame(StackFrame sf);

        /// <summary>
        /// Walks the <see cref="StackTrace"/> starting at <paramref name="skipFrames"/> until <paramref name="check"/> returns <see langword="true"/> for the current <see cref="StackFrame"/>.
        /// </summary>
        /// <param name="check">Method delegate to use for checking against the current <see cref="StackFrame"/></param>
        /// <param name="skipFrames">Number of frames from which to start walking</param>
        /// <returns>The <see cref="StackFrame"/> that matched the <paramref name="check"/> or <see langword="null"/> if nothing was found.</returns>
        public static StackFrame WalkStackTrace(CheckStackFrame check, int skipFrames = 1)
        {
            StackTrace stackTrace = new StackTrace(skipFrames);
            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                if (check(frame))
                {
                    return frame;
                }
            }

            return null;
        }
    }
}
