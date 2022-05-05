﻿using ChessEngine.Core.AI;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public Game Game { get; protected init; }

        protected IList<Movement> PossibleMovements { get; set; }

        protected IMovementGenerator MovementGenerator { get; init; }

        protected IMovementMigrator MovementMigrator { get; init; }

        protected IAttackDataGenerator AttackDataGenerator { get; init; }

        protected IGameHashing<ulong> GameHashing { get; init; }

        protected IEndGameChecker EndGameChecker { get; init; }

        protected Stack<Movement> MovementHistory { get; init; }

        protected IChessAI AI { get; init; }

        protected bool canUndo;
        public bool CanUndo
        {
            get => canUndo;
            protected set
            {
                if (canUndo != value)
                {
                    canUndo = value;
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

        public ICommand TreatChessMovementRequestCommand { get; protected init; }

        public ICommand TreatUndoRequestCommand { get; protected init; }

        public event EventHandler<MovementExecutionEventArgs>? MovementExecuted;

        public GameViewModel(Game game, IGameHashing<ulong> gameHashing)
        {
            Game = game;
            MovementHistory = new();
            TreatChessMovementRequestCommand = new RelayCommand<Movement>(TreatChessMovementRequest);
            TreatUndoRequestCommand = new RelayCommand(TreatUndoRequest);
            GameHashing = gameHashing;
            MovementGenerator = new MovementGenerator();
            MovementMigrator = new MovementMigrator(GameHashing, true);
            AttackDataGenerator = new AttackDataGenerator();
            EndGameChecker = new EndGameChecker();
            AI = new RandomChessAI();
            AttackData attackData = AttackDataGenerator.GenerateAttackData(Game);
            PossibleMovements = MovementGenerator.GenerateMovements(Game, attackData);
            CheckedPosition = attackData.IsCheck ? Game.Board[Game.CurrentPlayer] : BoardConsts.NoPosition;
        }

        protected void Do(Movement movement)
        {
            if (!PossibleMovements.Contains(movement))
            {
                throw new InvalidOperationException($"The movement {movement} is not legal.");
            }
            MovementMigrator.Up(Game, movement);
            MovementHistory.Push(movement);
            CanUndo = true;
            MovementExecuted?.Invoke(this, new MovementExecutionEventArgs(movement, MovementExecutionDirection.Up));
            AttackData attackData = AttackDataGenerator.GenerateAttackData(Game);
            PossibleMovements = MovementGenerator.GenerateMovements(Game, attackData);
            CheckedPosition = attackData.IsCheck ? Game.Board[Game.CurrentPlayer] : BoardConsts.NoPosition;
            EndGameType = EndGameChecker.CheckEndGame(Game, attackData, PossibleMovements).Type;
        }

        protected void Undo()
        {
            Movement movement = MovementHistory.Pop();
            MovementMigrator.Down(Game, movement);
            CanUndo = MovementHistory.Any();
            MovementExecuted?.Invoke(this, new MovementExecutionEventArgs(movement, MovementExecutionDirection.Down));
            AttackData attackData = AttackDataGenerator.GenerateAttackData(Game);
            PossibleMovements = MovementGenerator.GenerateMovements(Game, attackData);
            CheckedPosition = attackData.IsCheck ? Game.Board[Game.CurrentPlayer] : BoardConsts.NoPosition;
            EndGameType = EndGameType.GameIsNotFinished;
        }

        protected virtual void TreatChessMovementRequest(Movement request)
        {
            if (request.OldPosition == request.NewPosition) { return; }
            Piece piece = Game.Board[request.OldPosition];
            if (piece == PieceConsts.NoPiece) { return; }
            IEnumerable<Movement> movementsOnPosition = PossibleMovements.Where(move => move.OldPosition == request.OldPosition && move.NewPosition == request.NewPosition);
            Movement? movement = movementsOnPosition.Any()
                ? (movementsOnPosition.Count() == 1 ? movementsOnPosition.First() : movementsOnPosition.First(move => move.Flag == request.Flag))
                : null;
            if (movement is null) { return; }
            Do(movement.Value);
            if (PossibleMovements.Any())
            {
                Do(AI.SelectMovement(Game, PossibleMovements));
            }
        }

        protected void TreatUndoRequest()
        {
            if (CanUndo)
            {
                Undo();
            }
        }
    }
}
