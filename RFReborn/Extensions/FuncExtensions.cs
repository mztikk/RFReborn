using System;
using System.Threading;
using System.Threading.Tasks;

namespace RFReborn.Extensions
{
    /// <summary>
    /// Extends <see cref="Func{T}"/>.
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Executes a <see cref="Func{T, TResult}"/> until it returns true or exceeds the retry count
        /// </summary>
        /// <param name="func"><see cref="Func{T, TResult}"/> to execute/param>
        /// <param name="retries">Maximum number of retries</param>
        /// <param name="delayBetweenRetries">Delay between retries</param>
        public static async Task TryTilTrue(this Func<Task<bool>> func, int retries, int delayBetweenRetries)
        {
            while (retries > 0)
            {
                if (await func().ConfigureAwait(false))
                {
                    break;
                }

                await Task.Delay(delayBetweenRetries).ConfigureAwait(false);
                retries--;
            }
        }

        /// <summary>
        /// Executes a <see cref="Func{T, TResult}"/> until it returns true or exceeds the retry count
        /// </summary>
        /// <param name="func"><see cref="Func{T, TResult}"/> to execute/param>
        /// <param name="retries">Maximum number of retries</param>
        /// <param name="delayBetweenRetries">Delay between retries</param>
        public static void TryTilTrue(this Func<bool> func, int retries, int delayBetweenRetries)
        {
            while (retries > 0)
            {
                if (func())
                {
                    break;
                }

                Thread.Sleep(delayBetweenRetries);
                retries--;
            }
        }

        /// <summary>
        /// Executes a <see cref="Func{T, TResult}"/> until it returns true or exceeds the retry count
        /// </summary>
        /// <param name="func"><see cref="Func{T, TResult}"/> to execute/param>
        /// <param name="retries">Maximum number of retries</param>
        /// <param name="delayBetweenRetries">Delay between retries</param>
        public static async Task TryTilTrueAsync(this Func<bool> func, int retries, int delayBetweenRetries)
        {
            while (retries > 0)
            {
                if (func())
                {
                    break;
                }

                await Task.Delay(delayBetweenRetries).ConfigureAwait(false);
                retries--;
            }
        }
    }
}
