using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.Contracts;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Transposition.Zobrist;
using ChessEngine.Tests.Models;
using NUnit.Framework;
using System;
using System.Diagnostics;
using static ChessEngine.Tests.Tools.StockfishTestingTools;

namespace ChessEngine.Tests
{
    public class NodeExploration
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

        [TestCase(0, 1ul)]
        [TestCase(1, 20ul)]
        [TestCase(2, 400ul)]
        [TestCase(3, 8902ul)]
        [TestCase(4, 197281ul)]
        [TestCase(5, 4865609ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_01_NumNodes_At_InitialPosition(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load(FENConsts.StartFEN), "INIT_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 44ul)]
        [TestCase(2, 1486ul)]
        [TestCase(3, 62379ul)]
        [TestCase(4, 2103487ul)]
        [TestCase(5, 89941194ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_02_NumNodes_At_StevenEdwardsDebugPosition(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load(FENConsts.DebugFEN), "DEBUG_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 8ul)]
        [TestCase(2, 192ul)]
        [TestCase(3, 8355ul)]
        [TestCase(4, 206081ul)]
        [TestCase(5, 9296387ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_03_NumNodes_At_CustomDebugPosition01(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r6r/1b2k1bq/8/8/7B/8/8/R3K2R b KQ - 3 2"), "CUSTOM01_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 8ul)]
        [TestCase(2, 72ul)]
        [TestCase(3, 492ul)]
        [TestCase(4, 5380ul)]
        [TestCase(5, 36744ul)]
        [TestCase(6, 444954ul)]
        [TestCase(7, 3039234ul)]
        [TestCase(8, 39213236ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_04_NumNodes_At_CustomDebugPosition02(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/8/8/2k5/2pP4/8/B7/4K3 b - d3 0 3"), "CUSTOM02_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 5ul)]
        [TestCase(2, 259ul)]
        [TestCase(3, 11766ul)]
        [TestCase(4, 563603ul)]
        [TestCase(5, 24890051ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_05_NumNodes_At_CustomDebugPosition03(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r3k2r/p1pp1pb1/bn2Qnp1/2qPN3/1p2P3/2N5/PPPBBPPP/R3K2R b KQkq - 3 2"), "CUSTOM03_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 46ul)]
        [TestCase(2, 2079ul)]
        [TestCase(3, 89890ul)]
        [TestCase(4, 3894594ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_06_NumNodes_At_CustomDebugPosition04(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10"), "CUSTOM04_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 18ul)]
        [TestCase(2, 92ul)]
        [TestCase(3, 1670ul)]
        [TestCase(4, 10138ul)]
        [TestCase(5, 185429ul)]
        [TestCase(6, 1134888ul)]
        [TestCase(7, 20757544ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_07_NumNodes_At_CustomDebugPosition05(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("3k4/3p4/8/K1P4r/8/8/8/8 b - - 0 1"), "CUSTOM05_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 19ul)]
        [TestCase(2, 380ul)]
        [TestCase(3, 8163ul)]
        [TestCase(4, 182327ul)]
        [TestCase(5, 4364503ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_08_NumNodes_At_CustomDebugPosition06(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r1bqkbnr/pppppppp/n7/8/8/P7/1PPPPPPP/RNBQKBNR w KQkq - 2 2"), "CUSTOM06_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 44ul)]
        [TestCase(2, 2385ul)]
        [TestCase(3, 99756ul)]
        [TestCase(4, 5144430ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_09_NumNodes_At_CustomDebugPosition07(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("2kr3r/p1ppqpb1/bn2Qnp1/3PN3/1p2P3/2N5/PPPBBPPP/R3K2R b KQ - 3 2"), "CUSTOM07_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 39ul)]
        [TestCase(2, 1577ul)]
        [TestCase(3, 63647ul)]
        [TestCase(4, 2433142ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_10_NumNodes_At_CustomDebugPosition08(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("rnb2k1r/pp1Pbppp/2p5/q7/2B5/8/PPPQNnPP/RNB1K2R w KQ - 3 9"), "CUSTOM08_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 9ul)]
        [TestCase(2, 163ul)]
        [TestCase(3, 1349ul)]
        [TestCase(4, 23718ul)]
        [TestCase(5, 177964ul)]
        [TestCase(6, 3245821ul)]
        [TestCase(7, 24053172ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_11_NumNodes_At_CustomDebugPosition09(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("2r5/3pk3/8/2P5/8/2K5/8/8 w - - 5 4"), "CUSTOM09_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 44ul)]
        [TestCase(2, 1486ul)]
        [TestCase(3, 62379ul)]
        [TestCase(4, 2103487ul)]
        [TestCase(5, 89941194ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_12_NumNodes_At_CustomDebugPosition10(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8"), "CUSTOM10_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 13ul)]
        [TestCase(2, 102ul)]
        [TestCase(3, 1266ul)]
        [TestCase(4, 10276ul)]
        [TestCase(5, 135655ul)]
        [TestCase(6, 1015133ul)]
        [TestCase(7, 14047573ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_13_NumNodes_At_CustomDebugPosition11(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/8/4k3/8/2p5/8/B2P2K1/8 w - - 0 1"), "CUSTOM11_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 15ul)]
        [TestCase(2, 126ul)]
        [TestCase(3, 1928ul)]
        [TestCase(4, 13931ul)]
        [TestCase(5, 206379ul)]
        [TestCase(6, 1440467ul)]
        [TestCase(7, 21190412ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_14_NumNodes_At_CustomDebugPosition12(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/8/1k6/2b5/2pP4/8/5K2/8 b - d3 0 1"), "CUSTOM12_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 15ul)]
        [TestCase(2, 66ul)]
        [TestCase(3, 1198ul)]
        [TestCase(4, 6399ul)]
        [TestCase(5, 120330ul)]
        [TestCase(6, 661072ul)]
        [TestCase(7, 12762196ul)]
        [TestCase(8, 73450134ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_15_NumNodes_At_CustomDebugPosition13(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("5k2/8/8/8/8/8/8/4K2R w K - 0 1"), "CUSTOM13_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 16ul)]
        [TestCase(2, 71ul)]
        [TestCase(3, 1286ul)]
        [TestCase(4, 7418ul)]
        [TestCase(5, 141077ul)]
        [TestCase(6, 803711ul)]
        [TestCase(7, 15594314ul)]
        [TestCase(8, 91628014ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_16_NumNodes_At_CustomDebugPosition14(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("3k4/8/8/8/8/8/8/R3K3 w Q - 0 1"), "CUSTOM14_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 26ul)]
        [TestCase(2, 1141ul)]
        [TestCase(3, 27826ul)]
        [TestCase(4, 1274206ul)]
        [TestCase(5, 31912360ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_17_NumNodes_At_CustomDebugPosition15(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r3k2r/1b4bq/8/8/8/8/7B/R3K2R w KQkq - 0 1"), "CUSTOM15_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 44ul)]
        [TestCase(2, 1494ul)]
        [TestCase(3, 50509ul)]
        [TestCase(4, 1720476ul)]
        [TestCase(5, 58773923ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_18_NumNodes_At_CustomDebugPosition16(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("r3k2r/8/3Q4/8/8/5q2/8/R3K2R b KQkq - 0 1"), "CUSTOM16_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 11ul)]
        [TestCase(2, 133ul)]
        [TestCase(3, 1442ul)]
        [TestCase(4, 19174ul)]
        [TestCase(5, 266199ul)]
        [TestCase(6, 3821001ul)]
        [TestCase(7, 60651209ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_19_NumNodes_At_CustomDebugPosition17(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("2K2r2/4P3/8/8/8/8/8/3k4 w - - 0 1"), "CUSTOM17_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 29ul)]
        [TestCase(2, 165ul)]
        [TestCase(3, 5160ul)]
        [TestCase(4, 31961ul)]
        [TestCase(5, 1004658ul)]
        [TestCase(6, 6334638ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_20_NumNodes_At_CustomDebugPosition18(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/8/1P2K3/8/2n5/1q6/8/5k2 b - - 0 1"), "CUSTOM18_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 9ul)]
        [TestCase(2, 40ul)]
        [TestCase(3, 472ul)]
        [TestCase(4, 2661ul)]
        [TestCase(5, 38983ul)]
        [TestCase(6, 217342ul)]
        [TestCase(7, 3742283ul)]
        [TestCase(8, 20625698ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_21_NumNodes_At_CustomDebugPosition19(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("4k3/1P6/8/8/8/8/K7/8 w - - 0 1"), "CUSTOM19_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 6ul)]
        [TestCase(2, 27ul)]
        [TestCase(3, 273ul)]
        [TestCase(4, 1329ul)]
        [TestCase(5, 18135ul)]
        [TestCase(6, 92683ul)]
        [TestCase(7, 1555980ul)]
        [TestCase(8, 8110830ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_22_NumNodes_At_CustomDebugPosition20(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/P1k5/K7/8/8/8/8/8 w - - 0 1"), "CUSTOM20_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 2ul)]
        [TestCase(2, 6ul)]
        [TestCase(3, 13ul)]
        [TestCase(4, 63ul)]
        [TestCase(5, 382ul)]
        [TestCase(6, 2217ul)]
        [TestCase(7, 15453ul)]
        [TestCase(8, 93446ul)]
        [TestCase(9, 998319ul)]
        [TestCase(10, 5966690ul)]
        [TestCase(11, 85822924ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_23_NumNodes_At_CustomDebugPosition21(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("K1k5/8/P7/8/8/8/8/8 w - - 0 1"), "CUSTOM21_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 10ul)]
        [TestCase(2, 25ul)]
        [TestCase(3, 268ul)]
        [TestCase(4, 926ul)]
        [TestCase(5, 10857ul)]
        [TestCase(6, 43261ul)]
        [TestCase(7, 567584ul)]
        [TestCase(8, 2518905ul)]
        [TestCase(9, 37109897ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_24_NumNodes_At_CustomDebugPosition22(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/k1P5/8/1K6/8/8/8/8 w - - 0 1"), "CUSTOM22_POS", depth, expectedNumNodes);
        }

        [Test]
        [TestCase(0, 1ul)]
        [TestCase(1, 37ul)]
        [TestCase(2, 183ul)]
        [TestCase(3, 6559ul)]
        [TestCase(4, 23527ul)]
        [TestCase(5, 811573ul)]
        [TestCase(6, 3114998ul)]
        [Parallelizable(ParallelScope.All)]
        public void Test_25_NumNodes_At_CustomDebugPosition23(int depth, ulong expectedNumNodes)
        {
            VerifyNumNodes(FENGameLoader.Load("8/8/2k5/5q2/5n2/8/5K2/8 b - - 0 1"), "CUSTOM23_POS", depth, expectedNumNodes);
        }

        protected void VerifyNumNodes(Game game, string testIdentifier, int depth, ulong expectedNumNodes)
        {
            DateTime start = DateTime.Now;
            ulong numNodes = GetAllMovements(game, new TestMovementMigrator(MovementMigrator), MovementGenerator, AttackDataGenerator, depth);
            DateTime end = DateTime.Now;
            if (expectedNumNodes == numNodes)
            {
                Debug.WriteLine($"{testIdentifier} > Depth: {depth}, Result: {numNodes} position(s) found, Time: {end - start}, Status: OK");
            }
            else
            {
                Debug.Fail($"{testIdentifier} > Depth: {depth}, Result: {numNodes} position(s) found, Time: {end - start}, Status: ERROR, Expected: {expectedNumNodes}");
            }
            Assert.AreEqual(expectedNumNodes, numNodes);
        }
    }
}