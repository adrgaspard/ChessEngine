﻿using ChessEngine.Core.Utils;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.Utils;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        protected Clock Clock { get; init; }

        public TimeSpan RemainingTime => Clock.RemainingTime;

        public ClockParameters ClockParameters { get; protected init; }

        public bool IsInfiniteTime => ClockParameters == ClockParametersConsts.InfiniteTime;

        public ICommand StartCommand { get; protected init; }

        public ICommand PauseCommand { get; protected init; }

        public ICommand IncrementCommand { get; protected init; }

        public ICommand ResetCommand { get; protected init; }

        public event CountdownFinishedEventHandler? CountdownFinished;

        public ClockViewModel(ClockParameters clockParameters, int refreshRateInMs = 50)
        {
            ClockParameters = clockParameters;
            Clock = new(refreshRateInMs);
            Reset();
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
            Clock.Pause();
        }

        protected void Increment()
        {
            bool active = Clock.IsActivated;
            Clock.Pause();
            Clock.Add(ClockParameters.IncrementTime);
            if (active)
            {
                Clock.Start();
            }
        }

        protected void Reset()
        {
            Clock.Pause();
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
