using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChessEngine.Tests.Tools
{
    public static class StockfishTestingTools
    {
        private const string OrderedFileNames = "abcdefgh";
        private const string OrderedRankNames = "12345678";

        public static ulong GetAllMovements(Game game, IMovementMigrator movementMigrator, IMovementGenerator movementGenerator, IAttackDataGenerator attackDataGenerator, int depth)
        {
            if (depth <= 0)
            {
                return (ulong)(depth == 0 ? 1 : 0);
            }
            IList<Movement> movements = movementGenerator.GenerateMovements(game, attackDataGenerator.GenerateAttackData(game));
            ulong numPositions = 0;
            foreach (Movement movement in movements)
            {
                movementMigrator.Up(game, movement);
                numPositions += GetAllMovements(game, movementMigrator, movementGenerator, attackDataGenerator, depth - 1);
                movementMigrator.Down(game, movement);
            }
            return numPositions;
        }

        public static void FindError(Game game, IMovementGenerator movementGenerator, IMovementMigrator movementMigrator, IAttackDataGenerator attackDataGenerator)
        {
            // ---------------------------------------- Modify this section only ----------------------------------------
            IList<Tuple<string, ulong>> stockfishAnalysis = BuildStockfishAnalysis(""); // Change this by the result of a stockfish analysis
            int totalDepth = 4; // Change total depth of searching and eventually apply some movements.
            //MovementMigrator.Up(game, new(new Position(A, B), new Position(C, D), MovementFlag.None));
            // ----------------------------------------------------------------------------------------------------------
            IList<Tuple<string, Movement>> movements = movementGenerator.GenerateMovements(game, attackDataGenerator.GenerateAttackData(game)).Select(m => Tuple.Create(Convert(m), m)).ToList();
            long totalDifference = 0;
            foreach (Tuple<string, Movement>? movement in movements)
            {
                movementMigrator.Up(game, movement.Item2);
                ulong nodesFound = GetAllMovements(game, movementMigrator, movementGenerator, attackDataGenerator, totalDepth + 1 - game.StateHistory.Count);
                if (stockfishAnalysis.FirstOrDefault(tuple => tuple.Item1 == movement.Item1) is Tuple<string, ulong> result)
                {
                    long difference = (long)nodesFound - ((long)result.Item2);
                    totalDifference += difference;
                    Debug.WriteLineIf(difference != 0, $"Error on movement {movement.Item1} ({movement.Item2}) : {nodesFound} nodes found (expected {result.Item2}, difference = {difference}).");
                }
                else
                {
                    Debug.WriteLine($"Error on movement {movement.Item1} ({movement.Item2}) : movement should not exists.");
                }

                movementMigrator.Down(game, movement.Item2);
            }
            IEnumerable<string> shouldNotExist = stockfishAnalysis.Select(tuple => tuple.Item1).Where(m => !movements.Select(tuple => tuple.Item1).Contains(m));
            foreach (string? m in shouldNotExist)
            {
                Debug.WriteLine($"Error on movement {m} ({ConvertBackToMovement(m)}) : movement should exists.");
            }
            Debug.WriteLine($"Tests executed, {totalDifference} difference than the stockfish analysis (program may be incorrect even if the number is 0).");
        }

        private static string Convert(Movement movement)
        {
            string flagInfo = "";
            switch (movement.Flag)
            {
                case MovementFlag.PawnPromotionToBishop:
                    flagInfo = "b";
                    break;
                case MovementFlag.PawnPromotionToKnight:
                    flagInfo = "n";
                    break;
                case MovementFlag.PawnPromotionToQueen:
                    flagInfo = "q";
                    break;
                case MovementFlag.PawnPromotionToRook:
                    flagInfo = "r";
                    break;
                case MovementFlag.KingCastling:
                case MovementFlag.PawnEnPassantCapture:
                case MovementFlag.PawnAllPromotions:
                case MovementFlag.None:
                case MovementFlag.PawnPush:
                default:
                    break;
            }
            return $"{Convert(movement.OldPosition)}{Convert(movement.NewPosition)}{flagInfo}";
        }

        private static string Convert(Position position)
        {
            return position == BoardConsts.NoPosition ? "--" : $"{OrderedFileNames[position.File]}{OrderedRankNames[position.Rank]}";
        }

        private static Position ConvertBackToPosition(string str)
        {
            sbyte rank = sbyte.MinValue;
            switch (str[0])
            {
                case 'a':
                    rank = 0;
                    break;
                case 'b':
                    rank = 1;
                    break;
                case 'c':
                    rank = 2;
                    break;
                case 'd':
                    rank = 3;
                    break;
                case 'e':
                    rank = 4;
                    break;
                case 'f':
                    rank = 5;
                    break;
                case 'g':
                    rank = 6;
                    break;
                case 'h':
                    rank = 7;
                    break;
            }
            sbyte file = (sbyte)(sbyte.Parse(str[1].ToString()) - 1);
            return new(rank, file);
        }

        private static Movement ConvertBackToMovement(string str)
        {
            MovementFlag flag = MovementFlag.None;
            if (str.Length > 4)
            {
                switch (str[4])
                {
                    case 'b':
                        flag = MovementFlag.PawnPromotionToBishop;
                        break;
                    case 'n':
                        flag = MovementFlag.PawnPromotionToKnight;
                        break;
                    case 'q':
                        flag = MovementFlag.PawnPromotionToQueen;
                        break;
                    case 'r':
                        flag = MovementFlag.PawnPromotionToRook;
                        break;
                }
            }
            return new(ConvertBackToPosition(str[..2]), ConvertBackToPosition(str.Substring(2, 2)), flag);
        }

        private static IList<Tuple<string, ulong>> BuildStockfishAnalysis(string stockFishResult)
        {
            List<Tuple<string, ulong>> stockfishAnalysis = new();
            foreach (string str in stockFishResult.Split(Environment.NewLine))
            {
                string[] row = str.Split(" ");
                string movement = row[0].Replace(":", "");
                ulong numNodes = ulong.Parse(row[1]);
                stockfishAnalysis.Add(new(movement, numNodes));
            }
            return stockfishAnalysis;
        }
    }
}
