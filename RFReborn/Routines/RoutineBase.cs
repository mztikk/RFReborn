using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RFReborn.Routines
{
    /// <summary>
    /// Provides methods and base for a routinely called <see cref="Tick"/> method
    /// </summary>
    public abstract class RoutineBase : IRoutine
    {
        private DateTime _lastRun { get; set; }
        private TimeSpan _cooldownTime { get; }

        /// <summary>
        /// Constructs a new <see cref="RoutineBase"/> with a given Cooldown Time
        /// </summary>
        /// <param name="cooldownTime">Cooldown time between <see cref="Tick"/> Calls</param>
        protected RoutineBase(TimeSpan cooldownTime) => _cooldownTime = cooldownTime;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TimeSpan Cooldown()
        {
            TimeSpan diff = DateTime.UtcNow - _lastRun;
            if (diff >= _cooldownTime)
            {
                return TimeSpan.Zero;
            }

            return _cooldownTime - diff;
        }

        /// <summary>
        /// Will be called on <see cref="Tick"/> when its ready
        /// </summary>
        protected abstract Task OnTick();

        /// <inheritdoc />
        public async Task Tick()
        {
            if (!Ready())
            {
                return;
            }

            try
            {
                await OnTick().ConfigureAwait(false);
            }
            finally
            {
                _lastRun = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Will be called when <see cref="Kill"/> is called
        /// </summary>
        protected abstract Task Shutdown();

        /// <inheritdoc />
        public async Task Kill() => await Shutdown().ConfigureAwait(false);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool Ready() => Cooldown() == TimeSpan.Zero;

        #region Utils
        /// <summary>
        /// Forces the lower minute on a <see cref="DateTime"/> to be on a given interval of <paramref name="min"/>
        /// </summary>
        /// <param name="time"><see cref="DateTime"/> to base on</param>
        /// <param name="min">Interval to set</param>
        /// <param name="allowSubtraction">If this is set to <see langword="true"/> it will also go back to the last higher minute instead of only rolling over</param>
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
