using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;
using ChessEngine.UI.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public WPFPositionViewModel(WPFGameViewModel gameVM, Position position)
        {
            GameVM = gameVM;
            Position = position;
            GameVM.MovementExecuted += OnGameVMMovementExecuted;
            GameVM.PropertyChanged += OnGameVMPropertyChanged;
        }

        private void OnGameVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
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
