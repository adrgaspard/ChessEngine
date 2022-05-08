
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Abstractions
{
    public abstract class ResearcherBase : IResearcher
    {
        public const int NegativeInfinity = -2000000000;
        public const int PositiveInfinity = 2000000000;
        public const int LoseScore = -1000000000;
        public const int DrawScore = 0;
        public const int WinScore = 1000000000;
        protected const int MateMaximumDepth = 1000;

        public static readonly Movement NoMovement = new(BoardConsts.NoPosition, BoardConsts.NoPosition, MovementFlag.None);

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
        public static bool IsMateScore(int score)
        {
            return Math.Abs(score) > WinScore - MateMaximumDepth;
        }

        public static int NumPlaysToMateFromScore(int score)
        {
            return WinScore - Math.Abs(score);
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
