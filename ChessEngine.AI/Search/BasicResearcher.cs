﻿using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Search
{
    public class BasicResearcher : ResearcherBase
    {
        public BasicResearcher(IEvaluator evaluator, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementGenerator movementGenerator,
            IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst)
            : base(evaluator, attackDataGenerator, quietMovementGenerator, movementGenerator, movementMigrator, captureAnalyst)
        {
        }

        public override void LaunchResearch(Game game, int depth, CancellationToken token)
        {
            Search(game, game.CurrentPlayer, depth, depth, token);
        }

        public int Search(Game game, Colour aiSide, int initialDepth, int currentDepth, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return game.CurrentPlayer == aiSide ? LoseScore : WinScore;
            }
            if (currentDepth <= 0)
            {
                return Evaluator.Evaluate(game, game.CurrentPlayer);
            }
            AttackData attackData = AttackDataGenerator.GenerateAttackData(game);
            IList<Movement> movements = MovementGenerator.GenerateMovements(game, attackData);
            if (movements.Count == 0)
            {
                return attackData.IsCheck ? LoseScore : DrawScore;
            }
            int bestFound = LoseScore;
            foreach (Movement movement in movements)
            {
                MovementMigrator.Up(game, movement);
                int found = -Search(game, aiSide, initialDepth, currentDepth - 1, token);
                if (found >= bestFound)
                {
                    bestFound = found;
                    if (currentDepth == initialDepth)
                    {
                        BestFound = movement;
                    }
                }
                MovementMigrator.Down(game, movement);
            }
            return bestFound;
        }
    }
}
