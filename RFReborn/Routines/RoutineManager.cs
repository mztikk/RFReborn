using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace RFReborn.Routines
{
    /// <summary>
    /// Provides methods to manage routines, like starting and stopping
    /// </summary>
    public class RoutineManager
    {
        private readonly ICollection<RoutineBase> _routines;

        private readonly Timer _timer;

        /// <summary>
        /// Constructs a new <see cref="RoutineManager"/> with given <see cref="RoutineBase"/>s
        /// </summary>
        /// <param name="routineCollection"><see cref="RoutineBase"/>s to manage</param>
        /// <param name="interval">Interval between ticks</param>
        public RoutineManager(ICollection<RoutineBase> routineCollection, double interval)
        {
            _routines = routineCollection;
            _timer = new Timer();
            Interval = interval;
            _timer.AutoReset = true;
            _timer.Elapsed += OnTimer;
        }

        /// <summary>
        /// Constructs a new <see cref="RoutineManager"/> with given <see cref="RoutineBase"/>s and a default interval of 25
        /// </summary>
        /// <param name="routineCollection"><see cref="RoutineBase"/>s to manage</param>
        public RoutineManager(ICollection<RoutineBase> routineCollection) : this(routineCollection, 25) { }

        /// <summary>
        /// Constructs a new <see cref="RoutineManager"/> with given <see cref="RoutineBase"/>s through <see cref="RoutineManager.RoutineManager(ICollection{RoutineBase})"/>
        /// </summary>
        /// <param name="routines"><see cref="RoutineBase"/>s to manage</param>
        public RoutineManager(params RoutineBase[] routines) : this(routineCollection: routines) { }

        /// <summary>
        /// Interval between ticks
        /// </summary>
        public double Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        /// <summary>
        /// Adds a <see cref="RoutineBase"/> to manage
        /// </summary>
        /// <param name="routine"></param>
        public void AddRoutine(RoutineBase routine) => _routines.Add(routine);

        private async Task Tick()
        {
            foreach (RoutineBase routine in _routines)
            {
                await routine.Tick().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Starts routines
        /// </summary>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Start() => _timer.Start();
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Stops routine execution and calls <see cref="RoutineBase.Kill"/> on all
        /// </summary>
        public async Task Stop()
        {
            _timer.Stop();

            foreach (RoutineBase routine in _routines)
            {
                await routine.Kill().ConfigureAwait(false);
            }
        }

        private async void OnTimer(object source, ElapsedEventArgs e) => await Tick().ConfigureAwait(false);
    }
}
