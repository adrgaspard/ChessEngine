using System.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ChessEngine.Core.Utils
{
    public class Clock : INotifyPropertyChanged
    {
        protected readonly Timer Timer;

        protected TimeSpan remainingTime;
        public TimeSpan RemainingTime
        {
            get => remainingTime;
            protected set
            {
                if (remainingTime != value)
                {
                    if (value <= TimeSpan.Zero)
                    {
                        remainingTime = TimeSpan.Zero;
                    }
                    else
                    {
                        remainingTime = value;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingTime)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event CountdownFinishedEventHandler? CountdownFinished;

        public int RefreshRateInMs { get; protected init; }

        public Clock(int refreshRateInMs)
        {
            RefreshRateInMs = refreshRateInMs;
            Timer = new(refreshRateInMs);
            Timer.AutoReset = true;
            Timer.Enabled = false;
            Timer.Elapsed += OnTimerElapsed;
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        public void Set(TimeSpan timeSpan)
        {
            if (Timer.Enabled)
            {
                throw new InvalidOperationException("The clock must be stopped to perform a change on it.");
            }
            RemainingTime = timeSpan;
        }

        public void Add(TimeSpan timeSpan)
        {
            Set(RemainingTime + timeSpan);
        }

        public void Remove(TimeSpan timeSpan)
        {
            Set(RemainingTime - timeSpan);
        }

        protected void OnTimerElapsed(object? sender, ElapsedEventArgs eventArgs)
        {
            RemainingTime -= DateTime.Now - eventArgs.SignalTime;
            if (RemainingTime <= TimeSpan.Zero)
            {
                Stop();
            }
        }
    }
}
