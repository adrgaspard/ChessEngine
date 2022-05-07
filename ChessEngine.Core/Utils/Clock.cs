using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ChessEngine.Core.Utils
{
    public class Clock : IDisposable, INotifyPropertyChanged
    {
        protected readonly Stopwatch Stopwatch;
        protected readonly Timer Timer;

        protected TimeSpan maxTime;
        protected TimeSpan MaxTime
        {
            get => maxTime;
            set
            {
                maxTime = value;
                PropertyChanged?.Invoke(this, new(nameof(RemainingTime)));
            }
        }

        public TimeSpan RemainingTime
        {
            get
            {
                if (MaxTime == TimeSpan.MaxValue)
                {
                    return MaxTime;
                }
                double remainingTime = MaxTime.TotalMilliseconds - Stopwatch.ElapsedMilliseconds;
                return remainingTime > 0 ? TimeSpan.FromMilliseconds(remainingTime) : TimeSpan.Zero;
            }
        }

        public bool IsActivated => Timer.Enabled;

        public event PropertyChangedEventHandler? PropertyChanged;

        public event CountdownFinishedEventHandler? CountdownFinished;

        public Clock(int refreshDelayInMs)
        {
            Stopwatch = new();
            Timer = new(refreshDelayInMs);
            Timer.Elapsed += OnTimerElapsed;
        }

        public void Start()
        {
            Timer.Start();
            Stopwatch.Start();
            PropertyChanged?.Invoke(this, new(nameof(IsActivated)));
        }

        public void Pause()
        {
            Timer.Stop();
            Stopwatch.Stop();
            PropertyChanged?.Invoke(this, new(nameof(IsActivated)));
        }

        public void Set(TimeSpan timeSpan)
        {
            if (Timer.Enabled)
            {
                throw new InvalidOperationException("The clock must be stopped to perform a change on it.");
            }
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentException("The timespan must represent a positive value.");
            }
            Stopwatch.Reset();
            MaxTime = timeSpan;
        }

        public void Add(TimeSpan timeSpan)
        {
            Set(RemainingTime + timeSpan);
        }

        public void Remove(TimeSpan timeSpan)
        {
            Set(RemainingTime - timeSpan);
        }

        public void Dispose()
        {
            Pause();
            Timer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void OnTimerElapsed(object? sender, ElapsedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, new(nameof(RemainingTime)));
            if (MaxTime.TotalMilliseconds <= Stopwatch.ElapsedMilliseconds)
            {
                Timer.Enabled = false;
                Stopwatch.Stop();
                PropertyChanged?.Invoke(this, new(nameof(IsActivated)));
                CountdownFinished?.Invoke(this, new());
            }
        }
    }
}
