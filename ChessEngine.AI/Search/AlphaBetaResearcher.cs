using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Search
{
    public class AlphaBetaResearcher : SortedResearcherBase
    {
        public AlphaBetaResearcher(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator,
            IMovementGenerator movementGenerator, IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst)
            : base(evaluator, sorter, attackDataGenerator, quietMovementGenerator, movementGenerator, movementMigrator, captureAnalyst)
        {
        }

        public override void LaunchResearch(Game game, int depth, CancellationToken token)
        {
            Search(game, game.CurrentPlayer, depth, depth, NegativeInfinity, PositiveInfinity, token);
        }

        public int Search(Game game, Colour aiSide, int initialDepth, int currentDepth, int alpha, int beta, CancellationToken token)
        {
            if (currentDepth <= 0)
            {
                return CaptureAnalyst.SearchCaptures(game, alpha, beta);
            }
            if (token.IsCancellationRequested)
            {
                return game.CurrentPlayer == aiSide ? LoseScore : WinScore;
            }
            AttackData attackData = AttackDataGenerator.GenerateAttackData(game);
            IList<Movement> movements = Sorter.Sort(game, attackData, MovementGenerator.GenerateMovements(game, attackData));
            if (movements.Count == 0)
            {
                return attackData.IsCheck ? LoseScore - currentDepth : DrawScore;
            }
            foreach (Movement movement in movements)
            {
                MovementMigrator.Up(game, movement);
                int evaluation = -Search(game, aiSide, initialDepth, currentDepth - 1, -beta, -alpha, token);
                MovementMigrator.Down(game, movement);
                if (evaluation >= beta)
                {
                    return beta;
                }
                if (evaluation > alpha || BestFound == NoMovement)
                {
                    alpha = evaluation;
                    if (currentDepth == initialDepth)
                    {
                        BestFound = movement;
                    }
                }
            }
            return alpha;
        }
    }
}

