using ChessEngine.Core.Castling;
using ChessEngine.Core.Castling.Tools;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Environment.Tools;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Match;

namespace ChessEngine.Core.Interactions.Migration
{
    public partial class MovementMigrator : IMovementMigrator
    {
        public void Down(Game game, Movement movement)
        {
            ulong hash = game.Hash;
            CastlingState oldCastleState = game.State.CastlingState;
            Piece capturedPiece = game.State.CapturedPiece;
            bool isEnPassant = movement.Flag == MovementFlag.PawnEnPassantCapture;
            bool isPromotion = (movement.Flag & MovementFlag.PawnAllPromotions) != MovementFlag.None;
            PieceType targetPositionPieceType = game.Board[movement.NewPosition].Type;
            PieceType movedPieceType = isPromotion ? PieceType.Pawn : targetPositionPieceType;
            game.CurrentPlayer = game.OpponentPlayer;
            hash = GameHashing.IncrementHashOnCurrentPlayerUpdate(hash);
            hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.OldPosition, new(movedPieceType, game.CurrentPlayer));
            hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.NewPosition, new(targetPositionPieceType, game.CurrentPlayer));
            Position oldEnPassantTarget = game.State.EnPassantTarget;
            if (oldEnPassantTarget != BoardConsts.NoPosition)
            {
                hash = GameHashing.IncrementHashOnPawnPushUpdate(hash, oldEnPassantTarget);
            }
            if (capturedPiece.Type != PieceType.None && !isEnPassant)
            {
                hash = GameHashing.IncrementHashOnBoardUpdate(hash, movement.NewPosition, capturedPiece);
            }
            game.Board[movement.OldPosition] = new(movedPieceType, game.CurrentPlayer);
            game.Board[movement.NewPosition] = capturedPiece;
            if (isEnPassant)
            {
                Position enPassantPosition = new((sbyte)(movement.NewPosition.Rank + BoardConsts.PawnDirections[game.OpponentPlayer]), movement.NewPosition.File);
                game.Board[movement.NewPosition] = PieceConsts.NoPiece;
                game.Board[enPassantPosition] = capturedPiece;
                hash = GameHashing.IncrementHashOnBoardUpdate(hash, enPassantPosition, new(PieceType.Pawn, game.OpponentPlayer));
            }
            else if (movement.Flag == MovementFlag.KingCastling)
            {
                CastlingSide castlingSide = CastlingConsts.CastlingSideByKingFileAfterCastlingIndexes[movement.NewPosition.File];
                sbyte castlingRookInitialFile = CastlingConsts.RookInitialFileByCastlingSide[castlingSide];
                sbyte castlingRookFileAfterCastling = CastlingConsts.RookFileAfterCastlingByCastlingSide[castlingSide];
                Piece rook = new(PieceType.Rook, game.CurrentPlayer);
                Position rookInitialPosition = new(movement.NewPosition.Rank, castlingRookInitialFile);
                Position rookPositionAfterCastling = new(movement.NewPosition.Rank, castlingRookFileAfterCastling);
                game.Board[rookPositionAfterCastling] = PieceConsts.NoPiece;
                game.Board[rookInitialPosition] = rook;
                hash = GameHashing.IncrementHashOnBoardUpdate(hash, rookInitialPosition, rook);
                hash = GameHashing.IncrementHashOnBoardUpdate(hash, rookPositionAfterCastling, rook);
            }
            game.StateHistory.Pop();
            game.State = game.StateHistory.Peek();
            Position newEnPassantTarget = game.State.EnPassantTarget;
            if (newEnPassantTarget != BoardConsts.NoPosition)
            {
                hash = GameHashing.IncrementHashOnPawnPushUpdate(hash, newEnPassantTarget);
            }
            CastlingState newCastleState = game.State.CastlingState;
            if (newCastleState != oldCastleState)
            {
                hash = GameHashing.IncrementHashOnCastlingStateUpdate(hash, oldCastleState);
                hash = GameHashing.IncrementHashOnCastlingStateUpdate(hash, newCastleState);
            }
            game.TotalMoveCounter--;
            game.Hash = hash;
            if (RecordMovesInGameHistory)
            {
                if (game.HashsHistory.ContainsKey(hash))
                {
                    game.HashsHistory[hash]--;
                }
            }
        }
    }
}
