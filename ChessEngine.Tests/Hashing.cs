using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.Contracts;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Transposition.Zobrist;
using NUnit.Framework;
using System.Collections.Generic;

namespace ChessEngine.Tests
{
    public class Hashing
    {
        public IGameHashing<ulong> GameHashing { get; protected set; }

        public IGameLoader<string> FENGameLoader { get; protected set; }

        public IMovementMigrator MovementMigrator { get; protected set; }

        public IMovementGenerator MovementGenerator { get; protected set; }

        public IAttackDataGenerator AttackDataGenerator { get; protected set; }

        [SetUp]
        public void Setup()
        {
            GameHashing = new ZobristHashing();
            FENGameLoader = new FENGameLoader(GameHashing);
            MovementMigrator = new MovementMigrator(GameHashing, true);
            MovementGenerator = new MovementGenerator(PromotionGenerationType.AllPromotions, true);
            AttackDataGenerator = new AttackDataGenerator();
        }

        [Test]
        [TestCase(FENConsts.DebugFEN, 4)]
        [Parallelizable(ParallelScope.All)]
        public void Test_01_HashCorrectness(string fenData, int depth)
        {
            VerifyHashOnAllMovements(FENGameLoader.Load(fenData), MovementMigrator, MovementGenerator, AttackDataGenerator, depth);
        }

        protected static void VerifyHashOnAllMovements(Game game, IMovementMigrator movementMigrator, IMovementGenerator movementGenerator, IAttackDataGenerator attackDataGenerator, int depth)
        {
            if (depth <= 0)
            {
                return;
            }
            IList<Movement> movements = movementGenerator.GenerateMovements(game, attackDataGenerator.GenerateAttackData(game));
            foreach (Movement movement in movements)
            {
                ulong initialHash = game.Hash;
                movementMigrator.Up(game, movement);
                Assert.AreNotEqual(initialHash, game.Hash);
                VerifyHashOnAllMovements(game, movementMigrator, movementGenerator, attackDataGenerator, depth - 1);
                movementMigrator.Down(game, movement);
                Assert.AreEqual(initialHash, game.Hash);
            }
            return;
        }
    }
}
