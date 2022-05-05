using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels;
using ChessEngine.UI.WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChessEngine.UI.WPF.ViewModels
{
    public class WPFGameViewModel : GameViewModel
    {
        public IReadOnlyList<IReadOnlyList<WPFPositionViewModel>> PositionVMList { get; protected init; }
        public LocalHumanPlayerViewModel PlayerVM => (Players[Game.CurrentPlayer] as LocalHumanPlayerViewModel) ?? (LocalHumanPlayerViewModel)Players[Colour.White | Colour.Black];

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

        protected bool needPromotionTypeSpecification;
        public bool NeedPromotionTypeSpecification
        {
            get => needPromotionTypeSpecification;
            protected set
            {
                if (needPromotionTypeSpecification != value)
                {
                    needPromotionTypeSpecification = value;
                    RaisePropertyChanged();
                }
            }
        }

        public WPFGameViewModel(IGameHashing<ulong> gameHashing) : base(new FENGameLoader(gameHashing).Load(FENConsts.StartFEN), gameHashing, new DispatcherService(App.Current.Dispatcher))
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
            MovementExecuted += OnMovementExecuted;
        }

        protected override void TreatDoRequest(Movement request)
        {
            IEnumerable<Movement> movementsOnPosition = PossibleMovements.Where(move => move.OldPosition == request.OldPosition && move.NewPosition == request.NewPosition);
            if (movementsOnPosition.Count() > 1 && movementsOnPosition.All(move => (move.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None) && (request.Flag & MovementFlag.PawnAllPromotions) == MovementFlag.None)
            {
                NeedPromotionTypeSpecification = true;
            }
            else
            {
                NeedPromotionTypeSpecification = false;
                base.TreatDoRequest(request);
            }
        }

        protected void OnMovementExecuted(object? sender, MovementExecutionEventArgs eventArgs)
        {
            MarkedPositions = new List<Position>(2) { eventArgs.Movement.OldPosition, eventArgs.Movement.NewPosition };
        }
    }
}
