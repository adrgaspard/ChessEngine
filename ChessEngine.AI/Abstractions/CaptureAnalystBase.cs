using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Abstractions
{
    public abstract class CaptureAnalystBase : ICaptureAnalyst
    {
        public IEvaluator Evaluator { get; protected init; }

        public ISorter Sorter { get; protected init; }

        public IAttackDataGenerator AttackDataGenerator { get; protected init; }

        public IMovementGenerator QuietMovementGenerator { get; protected init; }

        public IMovementMigrator MovementMigrator { get; protected init; }

        protected CaptureAnalystBase(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementMigrator movementMigrator)
        {
            Evaluator = evaluator;
            Sorter = sorter;
            AttackDataGenerator = attackDataGenerator;
            QuietMovementGenerator = quietMovementGenerator;
            MovementMigrator = movementMigrator;
        }

        public abstract int SearchCaptures(Game game, int alpha, int beta);
    }
}
