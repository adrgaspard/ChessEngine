using ChessEngine.Core.Environment;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.MVVM.ViewModels.Abstractions;
using System;
using System.ComponentModel;
using System.Linq;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFPositionViewModel : ViewModelBase
    {
        public WPFGameViewModel GameVM { get; protected init; }

        public Position Position { get; protected init; }

        public Piece Piece => GameVM.Game.Board[Position];

        protected bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            protected set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected bool isMarked;
        public bool IsMarked
        {
            get => isMarked;
            protected set
            {
                if (isMarked != value)
                {
                    isMarked = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected bool isHighlighted;
        public bool IsHighlighted
        {
            get => isHighlighted;
            protected set
            {
                if (isHighlighted != value)
                {
                    isHighlighted = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            protected set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    RaisePropertyChanged();
                }
            }
        }

        public WPFPositionViewModel(WPFGameViewModel gameVM, Position position)
        {
            GameVM = gameVM;
            Position = position;
            GameVM.MovementExecuted += OnGameVMMovementExecuted;
            GameVM.PropertyChanged += OnGameVMPropertyChanged;
        }

        protected void OnGameVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(WPFGameViewModel.SelectedPosition):
                    IsSelected = GameVM.SelectedPosition == Position;
                    break;
                case nameof(WPFGameViewModel.HighlightedPositions):
                    IsHighlighted = GameVM.HighlightedPositions.Contains(Position);
                    break;
                case nameof(WPFGameViewModel.MarkedPositions):
                    IsMarked = GameVM.MarkedPositions.Contains(Position);
                    break;
                case nameof(GameViewModel.CheckedPosition):
                    IsChecked = GameVM.CheckedPosition == Position;
                    break;
                default:
                    break;
            }
        }

        protected void OnGameVMMovementExecuted(object? sender, MovementExecutionEventArgs eventArgs)
        {
            RaisePropertyChanged(nameof(Piece));
        }
    }
}
