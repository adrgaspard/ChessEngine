using ChessEngine.Core.Utils;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        public static readonly ClockParameters InfiniteClockParameters = new(TimeSpan.MaxValue, TimeSpan.MaxValue);

        public IReadOnlyList<ClockParameters> PredefinedClockParameters { get; protected init; }

        protected Clock Clock { get; init; }

        public TimeSpan RemainingTime => Clock.RemainingTime;

        protected ClockParameters clockParameters;
        public ClockParameters ClockParameters
        {
            get => clockParameters;
            protected set
            {
                if (clockParameters != value)
                {
                    clockParameters = value;
                    Reset();
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(RemainingTime));
                    RaisePropertyChanged(nameof(IsInfiniteTime));
                }
            }
        }

        public bool IsInfiniteTime => ClockParameters == InfiniteClockParameters;

        public ICommand StartCommand { get; protected init; }

        public ICommand PauseCommand { get; protected init; }

        public ICommand IncrementCommand { get; protected init; }

        public ICommand ResetCommand { get; protected init; }

        public event EventHandler<CountdownFinishedEventArgs>? CountdownFinished;

        public ClockViewModel()
        {
            PredefinedClockParameters = new ReadOnlyCollection<ClockParameters>(new List<ClockParameters>()
            {
                new(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(0)),
                new(TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(1)),
                new(TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(0)),
                new(TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(2)),
                new(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(0)),
                new(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(3)),
                new(TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(0)),
                new(TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(5)),
                new(TimeSpan.FromMinutes(15), TimeSpan.FromSeconds(10)),
                new(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(0)),
                new(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(20)),
                InfiniteClockParameters
            });
            Clock = new(100);
            Clock.PropertyChanged += OnClockPropertyChanged;
            Clock.CountdownFinished += OnClockCountdownFinished;
            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            IncrementCommand = new RelayCommand(Increment);
            ResetCommand = new RelayCommand(Reset);
        }

        protected void Start()
        {
            Clock.Start();
        }

        protected void Pause()
        {
            Clock.Stop();
        }

        protected void Increment()
        {
            Clock.Stop();
            Clock.Add(ClockParameters.IncrementTime);
            Clock.Start();
        }

        protected void Reset()
        {
            Clock.Stop();
            Clock.Set(ClockParameters.BaseTime);
        }

        protected void OnClockPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            RaisePropertyChanged(nameof(RemainingTime));
        }

        protected void OnClockCountdownFinished(object? sender, CountdownFinishedEventArgs eventArgs)
        {
            CountdownFinished?.Invoke(this, eventArgs);
        }
    }
}
