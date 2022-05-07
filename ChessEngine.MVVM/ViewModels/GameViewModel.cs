using ChessEngine.Core.AI;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Utils;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.Utils;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public Game Game { get; protected init; }

        public IReadOnlyList<Movement> PossibleMovements { get; protected set; }

        protected IMovementGenerator MovementGenerator { get; init; }

        protected IMovementMigrator MovementMigrator { get; init; }

        protected IAttackDataGenerator AttackDataGenerator { get; init; }

        protected IGameHashing<ulong> GameHashing { get; init; }

        protected IEndGameChecker EndGameChecker { get; init; }

        protected Stack<Movement> MovementHistory { get; init; }

        protected IReadOnlyDictionary<Colour, PlayerViewModel> Players { get; init; }

        public IReadOnlyDictionary<Colour, ClockViewModel> Clocks { get; protected init; }

        protected bool hasStarted;
        public bool HasStarted
        {
            get => hasStarted;
            protected set
            {
                if (hasStarted != value)
                {
                    hasStarted = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected EndGameType endGameType;
        public EndGameType EndGameType
        {
            get => endGameType;
            protected set
            {
                if (endGameType != value)
                {
                    endGameType = value;
                    if (EndGameType != EndGameType.GameIsNotFinished)
                    {
                        PossibleMovements = new List<Movement>(0);
                        foreach (PlayerViewModel playerVM in Players.Values)
                        {
                            playerVM.OnTurnToPlayEnded();
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        protected Position checkedPosition;
        public Position CheckedPosition
        {
            get => checkedPosition;
            protected set
            {
                if (checkedPosition != value)
                {
                    checkedPosition = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand StartCommand { get; protected init; }

        public ICommand InterruptCommand { get; protected init; }

        public event EventHandler<MovementExecutionEventArgs>? MovementExecuted;

        public GameViewModel(Game game, IGameHashing<ulong> gameHashing, Func<GameViewModel, IDictionary<Colour, PlayerViewModel>> playersFactory, Func<IDictionary<Colour, ClockViewModel>> clocksFactory)
        {
            HasStarted = false;
            Game = game;
            MovementHistory = new();
            StartCommand = new RelayCommand(Start);
            InterruptCommand = new RelayCommand(Interrupt);
            GameHashing = gameHashing;
            MovementGenerator = new MovementGenerator();
            MovementMigrator = new MovementMigrator(GameHashing, true);
            AttackDataGenerator = new AttackDataGenerator();
            EndGameChecker = new EndGameChecker();
            Players = new ReadOnlyDictionary<Colour, PlayerViewModel>(playersFactory.Invoke(this));
            Clocks = new ReadOnlyDictionary<Colour, ClockViewModel>(clocksFactory.Invoke());
            Clocks[Colour.White].CountdownFinished += OnWhiteClockVMCountdownFinished;
            Clocks[Colour.Black].CountdownFinished += OnBlackClockVMCountdownFinished;
            PossibleMovements = new List<Movement>(0);
        }

        protected void Start()
        {
            if (HasStarted)
            {
                throw new InvalidOperationException("The game is already started.");
            }
            HasStarted = true;
            UpdateGameInformations(false);
            UpdateViewModels();
        }

        protected void Interrupt()
        {
            if (!HasStarted)
            {
                throw new InvalidOperationException("The game is not started.");
            }
            PossibleMovements = new List<Movement>(0);
            CheckedPosition = BoardConsts.NoPosition;
            EndGameType = EndGameType.DrawOnInterruption;
            UpdateViewModels();
        }

        protected void Do(Movement movement)
        {
            if (!PossibleMovements.Contains(movement))
            {
                throw new InvalidOperationException($"The movement {movement} is not legal.");
            }
            MovementMigrator.Up(Game, movement);
            MovementHistory.Push(movement);
            MovementExecuted?.Invoke(this, new MovementExecutionEventArgs(movement, MovementExecutionDirection.Up));
            UpdateGameInformations(false);
        }

        protected virtual void TreatDoRequest(Movement request)
        {
            if (request.OldPosition == request.NewPosition) { return; }
            Piece piece = Game.Board[request.OldPosition];
            if (piece == PieceConsts.NoPiece) { return; }
            IEnumerable<Movement> movementsOnPosition = PossibleMovements.Where(move => move.OldPosition == request.OldPosition && move.NewPosition == request.NewPosition);
            Movement? movement = movementsOnPosition.Any() ? (movementsOnPosition.Count() == 1 ? movementsOnPosition.First() : movementsOnPosition.First(move => move.Flag == request.Flag)) : null;
            if (movement is null) { return; }
            Do(movement.Value);
            UpdateViewModels();
        }

        protected void UpdateGameInformations(bool isUndo)
        {
            AttackData attackData = AttackDataGenerator.GenerateAttackData(Game);
            PossibleMovements = new ReadOnlyCollection<Movement>(MovementGenerator.GenerateMovements(Game, attackData));
            CheckedPosition = attackData.IsCheck ? Game.Board[Game.CurrentPlayer] : BoardConsts.NoPosition;
            EndGameType = isUndo ? EndGameType.GameIsNotFinished : EndGameChecker.CheckEndGame(Game, attackData, PossibleMovements).Type;
        }

        protected void UpdateViewModels()
        {
            if (EndGameType == EndGameType.GameIsNotFinished)
            {
                Clocks[Game.OpponentPlayer].PauseCommand.Execute(null);
                if (MovementHistory.Count > 0)
                {
                    Clocks[Game.OpponentPlayer].IncrementCommand.Execute(null);
                    Players[Game.OpponentPlayer].OnTurnToPlayEnded();
                }
                Players[Game.OpponentPlayer].MovementFound -= OnCurrentPlayerPlayMovement;
                Players[Game.CurrentPlayer].MovementFound -= OnCurrentPlayerPlayMovement;
                Players[Game.CurrentPlayer].MovementFound += OnCurrentPlayerPlayMovement;
                Clocks[Game.CurrentPlayer].StartCommand.Execute(null);
                Players[Game.CurrentPlayer].OnTurnToPlayBegan();
            }
            else
            {
                Players[Game.CurrentPlayer].OnTurnToPlayEnded();
                Players[Game.OpponentPlayer].OnTurnToPlayEnded();
                foreach (ClockViewModel clockVM in Clocks.Values)
                {
                    clockVM.PauseCommand.Execute(null);
                }
            }
        }

        protected void OnCurrentPlayerPlayMovement(object? sender, MovementFindingEventArgs eventArgs)
        {
            TreatDoRequest(eventArgs.Movement);
        }

        protected void OnBlackClockVMCountdownFinished(object? sender, CountdownFinishedEventArgs eventArgs)
        {
            EndGameType = EndGameType.BlackHasFlagFall;
        }

        protected void OnWhiteClockVMCountdownFinished(object? sender, CountdownFinishedEventArgs eventArgs)
        {
            EndGameType = EndGameType.WhiteHasFlagFall;
        }
    }
}
