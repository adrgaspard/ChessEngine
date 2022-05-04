using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;
using ChessEngine.UI.WPF.Models;
using System;
using System.ComponentModel;
using System.Linq;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFChessPositionViewModel : ViewModelBase
    {
        public WPFChessGameViewModel GameVM { get; init; }

        public Position Position { get; init; }

        public DisplayablePiece? Piece => GameVM.Game.Board[Position] == PieceConsts.NoPiece ? null : new(GameVM.Game.Board[Position]);

        protected bool isHighLighted;
        public bool IsHighlighted
        {
            get => isHighLighted;
            protected set
            {
                if (isHighLighted != value)
                {
                    isHighLighted = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected bool isDraged;
        public bool IsDraged
        {
            get => isDraged;
            protected set
            {
                if (isDraged != value)
                {
                    isDraged = value;
                    RaisePropertyChanged();
                }
            }
        }

        public WPFChessPositionViewModel(WPFChessGameViewModel gameVM, Position position)
        {
            GameVM = gameVM;
            Position = position;
            GameVM.MovementExecuted += OnGameVMMovementMade;
            GameVM.PropertyChanged += OnGameVMPropertyChanged;
        }

        protected void VerifyDragedPosition()
        {
            IsDraged = GameVM.IsOnDragDrop && GameVM.DragedChessPosition == Position;
            if (IsDraged)
            {

            }
        }

        protected void VerifyHighlightedPositions()
        {
            IsHighlighted = GameVM.IsOnDragDrop && GameVM.HighlightedPositions.Contains(Position);
            if (IsHighlighted)
            {

            }
        }

        protected void OnGameVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(WPFChessGameViewModel.DragedChessPosition):
                    VerifyDragedPosition();
                    break;
                case nameof(WPFChessGameViewModel.HighlightedPositions):
                    VerifyHighlightedPositions();
                    break;
                case nameof(WPFChessGameViewModel.IsOnDragDrop):
                    VerifyDragedPosition();
                    VerifyHighlightedPositions();
                    break;
                default:
                    break;
            }
        }

        protected void OnGameVMMovementMade(object? sender, MovementExecutionEventArgs eventArgs)
        {
            //if (eventArgs.Movement.OldPosition == Position || eventArgs.Movement.NewPosition == Position ||
            //    ((eventArgs.Movement.Flag & SpecialMovementFlag.PawnEnPassantCapture) != 0 && (Piece is null || Piece.Type == ChessPieceType.Pawn)) ||
            //    ((eventArgs.Movement.Flag & SpecialMovementFlag.KingAllCastles) != 0 && (Piece is null || Piece.Type == ChessPieceType.Rook)))
            //{
            RaisePropertyChanged(nameof(Piece));
            //}
        }
    }
}
