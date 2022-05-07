using ChessEngine.AI.Contracts;
using ChessEngine.Core.Interactions.Contracts;

namespace ChessEngine.AI.Abstractions
{
    public abstract class SortedResearcherBase : ResearcherBase
    {
        public ISorter Sorter { get; protected init; }

        protected SortedResearcherBase(IEvaluator evaluator, ISorter sorter, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator,
            IMovementGenerator movementGenerator, IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst)
            : base(evaluator, attackDataGenerator, quietMovementGenerator, movementGenerator, movementMigrator, captureAnalyst)
        {
            Sorter = sorter;
        }
    }
}
