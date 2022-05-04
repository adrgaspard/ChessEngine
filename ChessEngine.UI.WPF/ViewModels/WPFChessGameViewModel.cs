using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFChessGameViewModel : GameViewModel
    {
        public IReadOnlyList<WPFChessRankViewModel> RankVMs { get; protected init; }

        public IReadOnlyList<PieceType> AvailablePromotionTypes { get; protected init; }

        protected PieceType selectedPromotionType;
        public PieceType SelectedpromotionType
        {
            get => selectedPromotionType;
            set
            {
                if (selectedPromotionType != value)
                {
                    if (AvailablePromotionTypes.Contains(value))
                    {
                        selectedPromotionType = value;
                        RaisePropertyChanged();
                    }
                    else
                    {
                        throw new ArgumentException($"The value {value} is not included in {nameof(AvailablePromotionTypes)}.");
                    }
                }
            }
        }

        protected Position? dragedChessPosition;
        public Position? DragedChessPosition
        {
            get => dragedChessPosition;
            set
            {
                if (dragedChessPosition != value)
                {
                    dragedChessPosition = value;
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

        protected bool isOnDragDrop;
        public bool IsOnDragDrop
        {
            get => isOnDragDrop;
            set
            {
                if (isOnDragDrop != value)
                {
                    isOnDragDrop = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ulong? Hash => Game?.Hash;

        public WPFChessGameViewModel(IGameHashing<ulong> gameHashing) : base(new FENGameLoader(gameHashing).Load(FENConsts.StartFEN), gameHashing)
        {
            AvailablePromotionTypes = new ReadOnlyCollection<PieceType>(new List<PieceType>() { PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight });
            SelectedpromotionType = PieceType.Queen;
            highlightedPositions = new List<Position>();
            List<WPFChessRankViewModel> rankVMs = new();
            for (sbyte i = 0; i < BoardConsts.BoardSize; i++)
            {
                List<WPFChessPositionViewModel> positionVMs = new();
                for (sbyte j = 0; j < BoardConsts.BoardSize; j++)
                {
                    positionVMs.Add(new(this, new(i, j)));
                }
                rankVMs.Add(new(this, i, positionVMs));
            }
            RankVMs = new ReadOnlyCollection<WPFChessRankViewModel>(rankVMs);
            MovementExecuted += OnMovementExecuted;
        }

        protected void OnMovementExecuted(object? sender, MovementExecutionEventArgs eventArgs)
        {
            RaisePropertyChanged(nameof(Hash));
        }

        protected override Movement? SelectMovement(Piece piece, IEnumerable<Movement> movements)
        {
            if (piece.Type == PieceType.Pawn && movements.Any())
            {
                IEnumerable<Movement> pawnPromotionMovements = movements.Where(move => (move.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None);
                if (pawnPromotionMovements.Any())
                {
                    switch (SelectedpromotionType)
                    {
                        case PieceType.Knight:
                            return pawnPromotionMovements.First(move => (move.Flag & MovementFlag.PawnPromotionToKnight) != MovementFlag.None);
                        case PieceType.Bishop:
                            return pawnPromotionMovements.First(move => (move.Flag & MovementFlag.PawnPromotionToBishop) != MovementFlag.None);
                        case PieceType.Rook:
                            return pawnPromotionMovements.First(move => (move.Flag & MovementFlag.PawnPromotionToRook) != MovementFlag.None);
                        case PieceType.Queen:
                            return pawnPromotionMovements.First(move => (move.Flag & MovementFlag.PawnPromotionToQueen) != MovementFlag.None);
                        default:
                            break;
                    }
                }
            }
            return base.SelectMovement(piece, movements);
        }
    }
}
