using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.CaptureAnalysis
{
    public class BasicCaptureAnalyst : CaptureAnalystBase
    {
        public BasicCaptureAnalyst(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementMigrator movementMigrator) : base(evaluator, sorter, attackDataGenerator, quietMovementGenerator, movementMigrator)
        {
        }

        public override int SearchCaptures(Game game, int alpha, int beta)
        {
            int evaluation = Evaluator.Evaluate(game, game.CurrentPlayer);
            if (evaluation >= beta)
            {
                return beta;
            }
            alpha = Math.Max(alpha, evaluation);
            AttackData attackData = AttackDataGenerator.GenerateAttackData(game);
            IList<Movement> captureMovements = Sorter.Sort(game, attackData, QuietMovementGenerator.GenerateMovements(game, attackData));
            foreach (Movement movement in captureMovements)
            {
                MovementMigrator.Up(game, movement);
                evaluation = -SearchCaptures(game, -beta, -alpha);
                MovementMigrator.Down(game, movement);
                if (evaluation >= beta)
                {
                    return beta;
                }
                alpha = Math.Max(alpha, evaluation);
            }
            return alpha;
        }
    }
}
