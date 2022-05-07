using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.CaptureAnalysis
{
    public class DummyCaptureAnalyst : CaptureAnalystBase
    {
        public DummyCaptureAnalyst(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementMigrator movementMigrator) : base(evaluator, sorter, attackDataGenerator, quietMovementGenerator, movementMigrator)
        {
        }

        public override int SearchCaptures(Game game, int alpha, int beta)
        {
            return Evaluator.Evaluate(game, game.CurrentPlayer);
        }
    }
}
