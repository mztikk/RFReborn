using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RFReborn.Routines
{
    public abstract class RoutineBase : IRoutine
    {
        #region Base Implementations
        private DateTime _lastCheck { get; set; }
        private TimeSpan _refreshDelay { get; set; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan Cooldown()
        {
            TimeSpan diff = DateTime.UtcNow - _lastCheck;
            if (diff >= _refreshDelay)
            {
                return TimeSpan.Zero;
            }

            return diff;
        }

        protected abstract Task OnTick();

        /// <inheritdoc />
        public async Task Tick()
        {
            if (Cooldown() != TimeSpan.Zero)
            {
                return;
            }

            try
            {
                await OnTick().ConfigureAwait(false);
            }
            finally
            {
                _lastCheck = ForceMinute(DateTime.UtcNow, 2, true);
            }
        }

        protected abstract Task Shutdown();

        /// <inheritdoc />
        public async Task Kill() => await Shutdown().ConfigureAwait(false);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool Ready() => Cooldown() == TimeSpan.Zero;

        #region Utils
        public static DateTime ForceMinute(DateTime time, int min, bool allowSubtraction = false)
        {
            if (min < 0 || min > 9)
            {
                throw new ArgumentException("Minute has to be in range of 0-9", nameof(min));
            }

            time = time.AddSeconds(-time.Second);
            int mod = time.Minute % 10;
            if (mod <= min || (allowSubtraction && mod < 5))
            {
                return time.AddMinutes(min - mod);
            }

            return time.AddMinutes(10 + min - mod);
        }
        #endregion
    }
}
