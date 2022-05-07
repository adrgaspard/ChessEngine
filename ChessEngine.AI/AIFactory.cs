using ChessEngine.AI.CaptureAnalysis;
using ChessEngine.AI.Contracts;
using ChessEngine.AI.Evaluation;
using ChessEngine.AI.PieceEvaluation;
using ChessEngine.AI.Search;
using ChessEngine.AI.Sort;
using ChessEngine.Core.AI;
using ChessEngine.Core.Interactions.Contracts;

namespace ChessEngine.AI
{
    public class AIFactory
    {
        protected IAttackDataGenerator AttackDataGenerator { get; init; }

        protected IMovementGenerator QuietMovementGenerator { get; init; }

        protected IMovementGenerator MovementGenerator { get; init; }

        protected IMovementMigrator MovementMigrator { get; init; }

        public AIFactory(IAttackDataGenerator attackDataGenerator, IMovementGenerator quietMovementGenerator, IMovementGenerator movementGenerator, IMovementMigrator movementMigrator)
        {
            AttackDataGenerator = attackDataGenerator;
            QuietMovementGenerator = quietMovementGenerator;
            MovementGenerator = movementGenerator;
            MovementMigrator = movementMigrator;
        }

        public IChessAI GetAI_Version_0(int delayInMs = 0)
        {
            return new RandomChessAI(AttackDataGenerator, MovementGenerator, delayInMs);
        }

        public IChessAI GetAI_Version_1(int searchDepth = 3)
        {
            IPieceEvaluator pieceEvaluator = new LarryKaufmanPieceEvaluator();
            ISorter sorter = new DummySorter();
            IEvaluator evaluator = new BasicEvaluator(pieceEvaluator);
            ICaptureAnalyst captureAnalyst = new DummyCaptureAnalyst(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementMigrator);
            IResearcher researcher = new BasicResearcher(evaluator, AttackDataGenerator, QuietMovementGenerator, MovementGenerator, MovementMigrator, captureAnalyst);
            return new BranchAndBoundAI(researcher, searchDepth);
        }

        public IChessAI GetAI_Version_2(int searchDepth = 3)
        {
            IPieceEvaluator pieceEvaluator = new LarryKaufmanPieceEvaluator();
            ISorter sorter = new DummySorter();
            IEvaluator evaluator = new BasicEvaluator(pieceEvaluator);
            ICaptureAnalyst captureAnalyst = new DummyCaptureAnalyst(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementMigrator);
            IResearcher researcher = new AlphaBetaResearcher(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementGenerator, MovementMigrator, captureAnalyst);
            return new BranchAndBoundAI(researcher, searchDepth);
        }

        public IChessAI GetAI_Version_3(int searchDepth = 3)
        {
            IPieceEvaluator pieceEvaluator = new LarryKaufmanPieceEvaluator();
            ISorter sorter = new BasicSorter(pieceEvaluator, 10, 1);
            IEvaluator evaluator = new BasicEvaluator(pieceEvaluator);
            ICaptureAnalyst captureAnalyst = new DummyCaptureAnalyst(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementMigrator);
            IResearcher researcher = new AlphaBetaResearcher(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementGenerator, MovementMigrator, captureAnalyst);
            return new BranchAndBoundAI(researcher, searchDepth);
        }

        public IChessAI GetAI_Version_4(int searchDepth = 3)
        {
            IPieceEvaluator pieceEvaluator = new LarryKaufmanPieceEvaluator();
            ISorter sorter = new BasicSorter(pieceEvaluator, 10, 1);
            IEvaluator evaluator = new BasicEvaluator(pieceEvaluator);
            ICaptureAnalyst captureAnalyst = new BasicCaptureAnalyst(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementMigrator);
            IResearcher researcher = new AlphaBetaResearcher(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementGenerator, MovementMigrator, captureAnalyst);
            return new BranchAndBoundAI(researcher, searchDepth);
        }

        public IChessAI GetAI_Version_5(int searchDepth = 3)
        {
            IPieceEvaluator pieceEvaluator = new LarryKaufmanPieceEvaluator();
            ISorter sorter = new BasicSorter(pieceEvaluator, 10, 1);
            IEvaluator evaluator = new EzEarlyAndLateGameEvaluator(pieceEvaluator, 10, 14, 4);
            ICaptureAnalyst captureAnalyst = new BasicCaptureAnalyst(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementMigrator);
            IResearcher researcher = new AlphaBetaResearcher(evaluator, sorter, AttackDataGenerator, QuietMovementGenerator, MovementGenerator, MovementMigrator, captureAnalyst);
            return new BranchAndBoundAI(researcher, searchDepth);
        }
    }
}
