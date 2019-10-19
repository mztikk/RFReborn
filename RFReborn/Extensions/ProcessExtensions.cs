using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Process"/>.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Waits asynchronously for the <see cref="Process"/> <paramref name="process"/> to exit.
        /// </summary>
        /// <param name="process">The process to wait for.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return immediately as canceled.</param>
        public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            void Process_Exited(object sender, EventArgs e) => tcs.TrySetResult(true);

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            try
            {
                if (process.HasExited)
                {
                    return;
                }

                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                {
                    await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                process.Exited -= Process_Exited;
            }
        }
    }
}
