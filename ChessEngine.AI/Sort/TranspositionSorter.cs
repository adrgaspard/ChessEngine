using ChessEngine.AI.Contracts;
using ChessEngine.AI.Transposition;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Generation.Tools;
using ChessEngine.Core.Match;

namespace ChessEngine.AI.Sort
{
    public class TranspositionSorter : ISorter
    {
        protected const int MaxMovements = 218;
        protected const int PositionControlledByOpponentPawnPenalty = 350;
        protected const int CapturedPieceValueMultiplier = 10;
        protected const int SameMovementThanTheHashedOneBonus = 10000;

        public IPieceEvaluator PieceEvaluator { get; protected init; }

        public TranspositionTable TranspositionTable { get; protected init; }

        protected int[] Scores { get; init; }

        public TranspositionSorter(IPieceEvaluator pieceEvaluator, TranspositionTable transpositionTable)
        {
            PieceEvaluator = pieceEvaluator;
            TranspositionTable = transpositionTable;
            Scores = new int[MaxMovements];
        }

        public IList<Movement> Sort(Game game, AttackData attackData, IList<Movement> movements)
        {
            Movement hashMovement = TranspositionTable.GetStoredMovement();
            int iterator = 0;
            foreach (Movement movement in movements)
            {
                int result = 0;
                PieceType movementPieceType = game.Board[movement.OldPosition].Type;
                PieceType capturedPieceType = movement.Flag == MovementFlag.PawnEnPassantCapture ? PieceType.Pawn : game.Board[movement.NewPosition].Type;
                if (capturedPieceType != PieceType.None)
                {
                    result = CapturedPieceValueMultiplier * PieceEvaluator.GetValue(capturedPieceType) - PieceEvaluator.GetValue(movementPieceType);
                }
                if (movementPieceType == PieceType.Pawn)
                {
                    if ((movement.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None)
                    {
                        PieceType promotedPawnType = movement.Flag switch
                        {
                            MovementFlag.PawnPromotionToBishop => PieceType.Bishop,
                            MovementFlag.PawnPromotionToKnight => PieceType.Knight,
                            MovementFlag.PawnPromotionToQueen => PieceType.Queen,
                            MovementFlag.PawnPromotionToRook => PieceType.Rook,
                            _ => PieceType.None,
                        };
                        result += PieceEvaluator.GetValue(promotedPawnType);
                    }
                }
                else
                {
                    if (attackData.IsPositionAttacked(movement.NewPosition))
                    {
                        result -= PositionControlledByOpponentPawnPenalty;
                    }
                }
                if (hashMovement == movement)
                {
                    result += SameMovementThanTheHashedOneBonus;
                }
                Scores[iterator] = result;
                iterator++;
            }
            for (int i = 0; i < movements.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    int swapIndex = j - 1;
                    if (Scores[swapIndex] < Scores[j])
                    {
                        (movements[j], movements[swapIndex]) = (movements[swapIndex], movements[j]);
                        (Scores[j], Scores[swapIndex]) = (Scores[swapIndex], Scores[j]);
                    }
                }
            }
            return movements;
        }
    }
}
