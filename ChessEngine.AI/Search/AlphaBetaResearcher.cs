using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;
using System.Diagnostics;

namespace ChessEngine.AI.Search
{
    public class AlphaBetaResearcher : ResearcherBase
    {
        public ISorter Sorter { get; protected init; }

        public int NodesSearched { get; protected set; }

        public AlphaBetaResearcher(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator,
            IMovementGenerator movementGenerator, IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst)
            : base(evaluator, attackDataGenerator, quietMovementGenerator, movementGenerator, movementMigrator, captureAnalyst)
        {
            Sorter = sorter;
        }

        public override void LaunchResearch(Game game, int depth, CancellationToken token)
        {
            NodesSearched = 0;
            DateTime start = DateTime.Now;
            Search(game, game.CurrentPlayer, depth, depth, NegativeInfinity, PositiveInfinity, token);
            Debug.WriteLine($"Search finished ! Depth: {depth}, Num. nodes searched: {NodesSearched}, Time elapsed: {DateTime.Now - start}");
        }

        public int Search(Game game, Colour aiSide, int initialDepth, int currentDepth, int alpha, int beta, CancellationToken token)
        {
            if (currentDepth <= 0)
            {
                NodesSearched++;
                return CaptureAnalyst.SearchCaptures(game, alpha, beta);
            }
            if (token.IsCancellationRequested)
            {
                NodesSearched++;
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

