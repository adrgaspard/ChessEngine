using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.Contracts;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Transposition.Zobrist;
using ChessEngine.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChessEngine.Tests
{
    public class CheckmateSearch
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
            MovementGenerator = new MovementGenerator(PromotionGenerationType.PromotionToQueenAndKnightOnly, true);
            AttackDataGenerator = new AttackDataGenerator();
        }

        [Test]
        [TestCase("2r2r2/6kp/3p4/3P4/4Pp2/5P1P/PP1pq1P1/4R2K b - - 0 1", 3)]
        [TestCase("r4br1/3b1kpp/1q1P4/1pp1RP1N/p7/6Q1/PPB3PP/2KR4 w - - 0 1", 3)]
        [TestCase("8/2p3N1/6p1/5PB1/pp2Rn2/7k/P1p2K1P/3r4 w - - 1 0", 5)]
        [TestCase("r4r1k/1pqb1B1p/p3p2B/2bpP2Q/8/1NP5/PP4PP/5R1K w - - 1 0", 5)]
        [TestCase("Q4R2/3kr3/1q3n1p/2p1p1p1/1p1bP1P1/1B1P3P/2PBK3/8 w - - 1 0", 5)]
        [TestCase("1Q6/5pk1/2p3p1/1p2N2p/1b5P/1b4n1/r5P1/2K5 b - - 0 1", 5)]
        [TestCase("rnb1kb1r/pp3ppp/2p5/4q3/4n3/3Q4/PPPB1PPP/2KR1BNR w - - 0 1", 5)]
        [Parallelizable(ParallelScope.All)]
        public void Test_01_CheckmateSearch(string fenData, int depth)
        {
            VerifyCheckmate(FENGameLoader.Load(fenData), fenData, depth);
        }

        protected void VerifyCheckmate(Game game, string testIdentifier, int depth)
        {
            DateTime start = DateTime.Now;
            bool checkmateFound = SearchCheckmate(game, new TestMovementMigrator(MovementMigrator), MovementGenerator, AttackDataGenerator, depth);
            DateTime end = DateTime.Now;
            if (checkmateFound)
            {
                Debug.WriteLine($"\"{testIdentifier}\" > Result: checkmate found in {depth} or less, Time: {end - start}, Status: OK");
            }
            else
            {
                Debug.Fail($"\"{testIdentifier}\" > Result: checkmate not found in {depth}, Time: {end - start}, Status: ERROR");
            }
            Assert.IsTrue(checkmateFound);
        }

        protected bool SearchCheckmate(Game game, IMovementMigrator movementMigrator, IMovementGenerator movementGenerator, IAttackDataGenerator attackDataGenerator, int depth)
        {
            IList<Movement> movements = movementGenerator.GenerateMovements(game, attackDataGenerator.GenerateAttackData(game));
            if (depth <= 0)
            {
                return !movements.Any() && AttackDataGenerator.GenerateAttackData(game).IsCheck;
            }
            foreach (Movement movement in movements)
            {
                movementMigrator.Up(game, movement);
                if (SearchCheckmate(game, movementMigrator, movementGenerator, attackDataGenerator, depth - 1))
                {
                    return true;
                }
                movementMigrator.Down(game, movement);
            }
            return false;
        }
    }
}
