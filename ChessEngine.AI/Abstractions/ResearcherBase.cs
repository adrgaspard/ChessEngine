
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Abstractions
{
    public abstract class ResearcherBase : IResearcher
    {
        protected const int NegativeInfinity = -2000000000;
        protected const int PositiveInfinity = 2000000000;
        protected const int LoseScore = -1000000000;
        protected const int DrawScore = 0;
        protected const int WinScore = 1000000000;
        protected static readonly Movement NoMovement = new(BoardConsts.NoPosition, BoardConsts.NoPosition, MovementFlag.None);

        public IEvaluator Evaluator { get; protected init; }

        public IAttackDataGenerator AttackDataGenerator { get; protected init; }

        public IMovementGenerator QuietMovementGenerator { get; protected init; }

        public IMovementGenerator MovementGenerator { get; protected init; }

        public IMovementMigrator MovementMigrator { get; protected init; }

        public ICaptureAnalyst CaptureAnalyst { get; protected init; }

        protected Movement BestFound { get; set; }

        protected ResearcherBase(IEvaluator evaluator, IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementGenerator movementGenerator,
            IMovementMigrator movementMigrator, ICaptureAnalyst captureAnalyst)
        {
            Evaluator = evaluator;
            AttackDataGenerator = attackDataGenerator;
            QuietMovementGenerator = quietMovementGenerator;
            MovementGenerator = movementGenerator;
            MovementMigrator = movementMigrator;
            CaptureAnalyst = captureAnalyst;
            BestFound = NoMovement;
        }

        public Movement Search(Game game, int depth, CancellationToken token)
        {
            BestFound = NoMovement;
            LaunchResearch(game, depth, token);
            return BestFound;
        }

        public abstract void LaunchResearch(Game game, int depth, CancellationToken token);
    }
}
