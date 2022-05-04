using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Match;
using static ChessEngine.Core.Interactions.Generation.Caching.MovementGenerationCache;

namespace ChessEngine.Core.Interactions.Generation.Tools
{
    public static class MovementGeneratorTools
    {
        public static bool AreOnDirectionAxe(Position directionAxe, Position startPosition, Position targetPosition)
        {
            Position direction = (targetPosition - startPosition).ToDirection();
            return direction == directionAxe || direction == -directionAxe;
        }

        public static bool IsCheckAfterEnPassant(Game game, AttackData attackData, Position startPosition, Position capturedPawnPosition)
        {
            Position kingPosition = game.Board[game.CurrentPlayer];
            if (attackData.AttackMapWithoutPawns.ContainsPosition(kingPosition))
            {
                return true;
            }
            Position horizontalOffset = capturedPawnPosition.File < kingPosition.File ? new(0, -1) : new(0, 1);
            for (sbyte i = 1; i <= GetRangeSizeWithOffset(kingPosition, horizontalOffset); i++)
            {
                Position positionIndex = kingPosition + (horizontalOffset * i);
                Piece piece = positionIndex == capturedPawnPosition || positionIndex == startPosition ? PieceConsts.NoPiece : game.Board[positionIndex];
                if (piece != PieceConsts.NoPiece)
                {
                    if (piece.Colour == game.OpponentPlayer && (piece.Type & (PieceType.Queen | PieceType.Rook)) != PieceType.None)
                    {
                        return true;
                    }
                    break;
                }
            }
            foreach (Position potentialPawnAttackPosition in GetPawnPartialCaptureMovements(kingPosition, game.CurrentPlayer, false).Select(movement => movement.NewPosition))
            {
                Piece potentialAttackingPawn = potentialPawnAttackPosition == capturedPawnPosition || potentialPawnAttackPosition == startPosition ? PieceConsts.NoPiece : game.Board[potentialPawnAttackPosition];
                if (potentialAttackingPawn.Type == PieceType.Pawn && potentialAttackingPawn.Colour == game.OpponentPlayer)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
