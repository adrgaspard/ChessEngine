using ChessEngine.AI.Abstractions;
using ChessEngine.AI.Contracts;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Generation.Tools;
using ChessEngine.Core.Match;
using System.ComponentModel;

namespace ChessEngine.AI.Sort
{
    public class BasicSorter : ScoringSorterBase
    {
        public int MovementPieceTypeMultiplier { get; protected init; }

        public int CapturedPieceTypeMultiplier { get; protected init; }

        public BasicSorter(IPieceEvaluator pieceEvaluator, int movementPieceTypeMultiplier, int capturedPieceTypeMultiplier) : base(pieceEvaluator, ListSortDirection.Descending)
        {
            MovementPieceTypeMultiplier = movementPieceTypeMultiplier;
            CapturedPieceTypeMultiplier = capturedPieceTypeMultiplier;
        }

        public override int GetMovementScore(Game game, AttackData attackData, Movement movement)
        {
            int result = 0;
            PieceType movementPieceType = game.Board[movement.OldPosition].Type;
            PieceType capturedPieceType = movement.Flag == MovementFlag.PawnEnPassantCapture ? PieceType.Pawn : game.Board[movement.NewPosition].Type;
            if (capturedPieceType != PieceType.None)
            {
                result = CapturedPieceTypeMultiplier * PieceEvaluator.GetValue(capturedPieceType) - MovementPieceTypeMultiplier * PieceEvaluator.GetValue(movementPieceType);
            }
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
            if (attackData.IsPositionAttacked(movement.NewPosition))
            {
                result -= PieceEvaluator.GetValue(movementPieceType);
            }
            return result;
        }
    }
}
