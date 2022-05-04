using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.MVVM.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFGameViewModel : GameViewModel
    {
        public IReadOnlyList<IReadOnlyList<WPFPositionViewModel>> PositionVMList { get; protected init; }

        protected Colour drageablePieces;
        public Colour DrageablePieces
        {
            get => drageablePieces;
            protected set
            {
                if (drageablePieces != value)
                {
                    drageablePieces = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected Colour pointOfView;
        public Colour PointOfView
        {
            get => pointOfView;
            protected set
            {
                if (pointOfView != value)
                {
                    pointOfView = value;
                    VerifyCanDrag();
                    RaisePropertyChanged();
                }
            }
        }

        protected Position selectedPosition;
        public Position SelectedPosition
        {
            get => selectedPosition;
            set
            {
                if (selectedPosition != value)
                {
                    selectedPosition = value;
                    RaisePropertyChanged();
                    HighlightedPositions = PossibleMovements.Where(movement => movement.OldPosition == value).Select(movement => movement.NewPosition).ToList();
                }
            }
        }

        protected IReadOnlyList<Position> highlightedPositions;
        public IReadOnlyList<Position> HighlightedPositions
        {
            get => highlightedPositions;
            protected set
            {
                if (highlightedPositions != value)
                {
                    highlightedPositions = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected IReadOnlyList<Position> markedPositions;
        public IReadOnlyList<Position> MarkedPositions
        {
            get => markedPositions;
            protected set
            {
                if (markedPositions != value)
                {
                    markedPositions = value;
                    RaisePropertyChanged();
                }
            }
        }

        public WPFGameViewModel(IGameHashing<ulong> gameHashing) : base(new FENGameLoader(gameHashing).Load(FENConsts.StartFEN), gameHashing)
        {
            List<IReadOnlyList<WPFPositionViewModel>> positionsVMList = new(BoardConsts.BoardSize);
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                List<WPFPositionViewModel> positionVMSubList = new(BoardConsts.BoardSize);
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    positionVMSubList.Add(new(this, new(i, j)));
                }
                positionsVMList.Add(new ReadOnlyCollection<WPFPositionViewModel>(positionVMSubList));
            }
            PositionVMList = new ReadOnlyCollection<IReadOnlyList<WPFPositionViewModel>>(positionsVMList);
            PointOfView = Colour.White;
            MovementExecuted += OnMovementExecuted;
        }

        protected void OnMovementExecuted(object? sender, MovementExecutionEventArgs eventArgs)
        {
            VerifyCanDrag();
            MarkedPositions = new List<Position>(2) { eventArgs.Movement.OldPosition, eventArgs.Movement.NewPosition };
        }

        protected void VerifyCanDrag()
        {
            DrageablePieces = Game.CurrentPlayer & PointOfView;
        }
    }
}
